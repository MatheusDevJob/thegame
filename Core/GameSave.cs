using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace thegame.Core;

public class GameSave
{
    public int Version { get; set; } = 1;
    public string CurrentMap { get; set; } = "city";
    public float PlayerLife { get; set; } = 75f;
    public Vector2 PlayerPosition { get; set; } = new(1200, 220);
    public List<string> EquipableIds { get; set; } =
    [
        "AxeTool",
        "PickaxeTool",
        "",
        "",
        "",
        "",
        "",
        ""
    ];
    public string ActiveEquipe { get; set; } = "";
    public int BagLevel { get; set; } = 4;
    public List<ItemStackSave> BagItems { get; set; } = [];
    public Dictionary<string, MapSave> Maps { get; set; } = [];

    public MapSave GetMapSave(string mapId)
    {
        if (!Maps.ContainsKey(mapId))
            Maps[mapId] = new MapSave();

        return Maps[mapId];
    }
}

public class ItemStackSave
{
    public string ItemId { get; set; } = "";
    public int Amount { get; set; } = 1;
}

public class MapSave
{
    public List<string> RemovedEntities { get; set; } = [];
    public List<WorldEntitySave> SpawnedEntities { get; set; } = [];
    public List<WorldItemSave> DroppedItems { get; set; } = [];
    public List<WorldTileSave> WorldTileState { get; set; } = [];

    public void SetTileState(WorldTileSave state)
    {
        WorldTileSave existing = WorldTileState.FirstOrDefault(tile =>
            tile.Layer == state.Layer &&
            tile.X == state.X &&
            tile.Y == state.Y
        );

        if (existing == null)
        {
            WorldTileState.Add(state);
            return;
        }

        existing.BaseTerrain = state.BaseTerrain;
        existing.State = state.State;
        existing.OverrideKey = state.OverrideKey;
    }
}

public class WorldEntitySave
{
    public string SaveId { get; set; } = "";
    public string EntityId { get; set; } = "";
    public float X { get; set; }
    public float Y { get; set; }
    public Dictionary<string, string> Data { get; set; } = [];
}

public class WorldItemSave
{
    public string SaveId { get; set; } = "";
    public string ItemId { get; set; } = "";
    public int Amount { get; set; } = 1;
    public float X { get; set; }
    public float Y { get; set; }
}

public class WorldTileSave
{
    public string Layer { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string BaseTerrain { get; set; }
    public string State { get; set; }
    public string OverrideKey { get; set; }
    public int Quantity { get; set; } = 1;
}