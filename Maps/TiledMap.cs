using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace thegame.Maps;

public class TiledMap
{
    private TiledMapData _map;
    private readonly Dictionary<TiledTilesetData, Texture2D> _textures = [];
    private readonly List<Rectangle> _collisionRectangles = [];
    public int TileWidth => _map.TileWidth;
    public int TileHeight => _map.TileHeight;
    public int Width => _map.Width;
    public int Height => _map.Height;
    public int PixelWidth => Width * TileWidth;
    public int PixelHeight => Height * TileHeight;

    public void Load(ContentManager content, string mapPath)
    {
        var fullMapPath = Path.GetFullPath(Path.Combine(content.RootDirectory, mapPath));
        var mapDir = Path.GetDirectoryName(fullMapPath)!;
        var contentRoot = Path.GetFullPath(content.RootDirectory);
        var json = File.ReadAllText(fullMapPath);

        _map = JsonSerializer.Deserialize<TiledMapData>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        foreach (var tileset in _map.Tilesets)
        {
            var imagePath = tileset.Image.Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullImagePath = Path.GetFullPath(Path.Combine(mapDir, imagePath));
            var relativeToContent = Path.GetRelativePath(contentRoot, fullImagePath);
            var assetName = Path.ChangeExtension(relativeToContent, null)!.Replace("\\", "/");

            _textures[tileset] = content.Load<Texture2D>(assetName);
        }

        LoadCollisionObjects();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var layer in _map.Layers)
        {
            if (layer.Type != "tilelayer" || !layer.Visible || layer.Data == null)
                continue;

            DrawTileLayer(spriteBatch, layer);
        }
    }

    public bool Collides(Rectangle hitbox)
    {
        foreach (var collision in _collisionRectangles)
        {
            if (hitbox.Intersects(collision))
                return true;
        }

        return false;
    }
    public IEnumerable<TiledObjectData> GetObjects(string layerName)
    {
        var layer = _map.Layers.FirstOrDefault(layer =>
            layer.Type == "objectgroup" &&
            layer.Name == layerName
        );

        if (layer == null || layer.Objects == null)
            return [];

        return layer.Objects;
    }
    private TiledTilesetData GetTileset(int gid)
    {
        return _map.Tilesets
            .Where(t => gid >= t.FirstGid)
            .OrderByDescending(t => t.FirstGid)
            .FirstOrDefault();
    }

    private static int ClearFlipFlags(int gid)
    {
        return gid & ~(unchecked((int)0x80000000) | 0x40000000 | 0x20000000);
    }

    private void LoadCollisionObjects()
    {
        _collisionRectangles.Clear();

        foreach (var layer in _map.Layers)
        {
            if (layer.Type != "objectgroup")
                continue;

            if (layer.Name != "Collisions" && layer.Name != "Colisoes")
                continue;

            if (layer.Objects == null)
                continue;

            foreach (var obj in layer.Objects)
            {
                _collisionRectangles.Add(new Rectangle(
                    (int)obj.X,
                    (int)obj.Y,
                    (int)obj.Width,
                    (int)obj.Height
                ));
            }
        }
    }

    public void DrawLayers(SpriteBatch spriteBatch, params string[] layerNames)
    {
        foreach (string layerName in layerNames)
            DrawLayer(spriteBatch, layerName);
    }

    public void DrawLayer(SpriteBatch spriteBatch, string layerName)
    {
        var layer = _map.Layers.FirstOrDefault(layer => layer.Name == layerName);

        if (layer == null || layer.Type != "tilelayer" || !layer.Visible || layer.Data == null)
            return;

        DrawTileLayer(spriteBatch, layer);
    }

    private void DrawTileLayer(SpriteBatch spriteBatch, TiledLayerData layer)
    {
        for (int y = 0; y < layer.Height; y++)
        {
            for (int x = 0; x < layer.Width; x++)
            {
                var index = y * layer.Width + x;
                var gid = layer.Data[index];

                if (gid == 0)
                    continue;

                gid = ClearFlipFlags(gid);

                var tileset = GetTileset(gid);

                if (tileset == null)
                    continue;

                var texture = _textures[tileset];
                var localId = gid - tileset.FirstGid;
                var columns = tileset.Columns;

                var sourceX = localId % columns * tileset.TileWidth;
                var sourceY = localId / columns * tileset.TileHeight;

                var source = new Rectangle(
                    sourceX,
                    sourceY,
                    tileset.TileWidth,
                    tileset.TileHeight
                );

                var destination = new Rectangle(
                    x * _map.TileWidth,
                    y * _map.TileHeight,
                    _map.TileWidth,
                    _map.TileHeight
                );

                spriteBatch.Draw(texture, destination, source, Color.White);
            }
        }
    }
}

public class TiledMapData
{
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("tilewidth")] public int TileWidth { get; set; }
    [JsonPropertyName("tileheight")] public int TileHeight { get; set; }
    [JsonPropertyName("layers")] public List<TiledLayerData> Layers { get; set; }
    [JsonPropertyName("tilesets")] public List<TiledTilesetData> Tilesets { get; set; }
}

public class TiledLayerData
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("visible")] public bool Visible { get; set; } = true;
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("data")] public List<int> Data { get; set; }
    [JsonPropertyName("objects")] public List<TiledObjectData> Objects { get; set; }
}
public class TiledObjectData
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("x")] public float X { get; set; }
    [JsonPropertyName("y")] public float Y { get; set; }
    [JsonPropertyName("width")] public float Width { get; set; }
    [JsonPropertyName("height")] public float Height { get; set; }
}
public class TiledTilesetData
{
    [JsonPropertyName("firstgid")] public int FirstGid { get; set; }
    [JsonPropertyName("image")] public string Image { get; set; }
    [JsonPropertyName("tilewidth")] public int TileWidth { get; set; }
    [JsonPropertyName("tileheight")] public int TileHeight { get; set; }
    [JsonPropertyName("columns")] public int Columns { get; set; }
    [JsonPropertyName("tilecount")] public int TileCount { get; set; }
}