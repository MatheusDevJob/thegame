using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using thegame.Maps;

namespace thegame.Core;

public static class SaveManager
{
    private const int CurrentVersion = 1;
    private const string SaveFileName = "save_03.json";
    private const int EquipableSlotCount = 8;
    private static GameContext context;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static string SaveDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "thegame",
        "Saves"
    );

    public static string SavePath => Path.Combine(SaveDirectory, SaveFileName);

    public static bool HasSave()
    {
        return File.Exists(SavePath);
    }

    public static GameSave LoadOrCreate(GameContext _context)
    {
        context = _context;
        return HasSave() ? Load() : CreateNewSave();
    }

    public static GameSave CreateNewSave()
    {
        return new GameSave
        {
            Version = CurrentVersion,
            CurrentMap = "city",
            PlayerLife = 100f,
            PlayerPosition = new Vector2(1200, 220),
            ActiveEquipe = "AxeTool",
            BagLevel = 4,
            BagItems =
            [
                new ItemStackSave
            {
                ItemId = "AxeTool",
                Amount = 1,
                ListIndex = 0
            },
            new ItemStackSave
            {
                ItemId = "PickaxeTool",
                Amount = 1,
                ListIndex = 1
            },
            new ItemStackSave
            {
                ItemId = "ShovelTool",
                Amount = 1,
                ListIndex = 2
            }
            ],
            Maps = new Dictionary<string, MapSave>
            {
                ["city"] = CreateInitialMapSave("city")
            }
        };
    }
    private static MapSave CreateInitialMapSave(string mapId)
    {
        TiledMap tiledMap = new();
        MapSave mapSave = new();
        tiledMap.Load(context.Content, "Maps/HomeMap.tmj");

        TiledLayerData objectsLayer = tiledMap.GetObjects("Objects");

        if (objectsLayer?.Objects == null)
            return mapSave;

        if (objectsLayer.Visible)
            foreach (TiledObjectData obj in objectsLayer.Objects)
            {
                string entityId = !string.IsNullOrWhiteSpace(obj.Type) ? obj.Type : obj.Name;

                if (string.IsNullOrWhiteSpace(entityId))
                    continue;

                mapSave.SpawnedEntities.Add(new WorldEntitySave
                {
                    SaveId = $"{mapId}:{obj.Id}",
                    EntityId = entityId,
                    X = obj.X,
                    Y = obj.Y,
                    Data = []
                });
            }

        return mapSave;
    }


    public static GameSave Load()
    {
        try
        {
            string json = File.ReadAllText(SavePath);
            SaveFileData data = JsonSerializer.Deserialize<SaveFileData>(json, JsonOptions);

            if (data == null)
                return CreateNewSave();

            return data.ToGameSave();
        }
        catch
        {
            TryBackupCorruptedSave();
            return CreateNewSave();
        }
    }

    public static void Save(GameSave save)
    {
        if (save == null)
            return;

        Directory.CreateDirectory(SaveDirectory);

        SaveFileData data = SaveFileData.FromGameSave(save);
        string json = JsonSerializer.Serialize(data, JsonOptions);

        File.WriteAllText(SavePath, json);
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }


    private static void TryBackupCorruptedSave()
    {
        try
        {
            if (!File.Exists(SavePath))
                return;

            string backupPath = SavePath + ".corrupted";
            File.Copy(SavePath, backupPath, true);
            File.Delete(SavePath);
        }
        catch
        {
        }
    }

    private class SaveFileData
    {
        public int Version { get; set; } = CurrentVersion;
        public string CurrentMap { get; set; } = "city";
        public float PlayerLife { get; set; }
        public float PlayerX { get; set; }
        public float PlayerY { get; set; }
        public string ActiveEquipe { get; set; } = "";
        public int BagLevel { get; set; }
        public List<ItemStackSave> BagItems { get; set; } = [];
        public Dictionary<string, MapSave> Maps { get; set; } = [];

        public static SaveFileData FromGameSave(GameSave save)
        {
            return new SaveFileData
            {
                Version = CurrentVersion,
                CurrentMap = save.CurrentMap ?? "city",
                PlayerLife = save.PlayerLife,
                PlayerX = save.PlayerPosition.X,
                PlayerY = save.PlayerPosition.Y,
                ActiveEquipe = save.ActiveEquipe ?? "",
                BagLevel = save.BagLevel,
                BagItems = save.BagItems ?? [],
                Maps = save.Maps ?? []
            };
        }

        public GameSave ToGameSave()
        {
            return new GameSave
            {
                Version = Version,
                CurrentMap = string.IsNullOrWhiteSpace(CurrentMap) ? "city" : CurrentMap,
                PlayerLife = PlayerLife,
                PlayerPosition = new Vector2(PlayerX, PlayerY),
                ActiveEquipe = ActiveEquipe ?? "",
                BagLevel = BagLevel <= 0 ? 4 : BagLevel,
                BagItems = BagItems ?? [],
                Maps = Maps ?? []
            };
        }
    }
}