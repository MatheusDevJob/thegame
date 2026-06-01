using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace thegame.Core;

public static class SaveManager
{
    private const int CurrentVersion = 1;
    private const string SaveFileName = "save_01.json";
    private static readonly List<string> tools = ["AxeTool", "PickaxeTool"];

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

    public static GameSave LoadOrCreate()
    {
        return HasSave() ? Load() : CreateNewSave();
    }

    public static GameSave CreateNewSave()
    {
        return new GameSave
        {
            Version = CurrentVersion,
            CurrentMap = "city",
            PlayerLife = 75f,
            PlayerPosition = new Vector2(1200, 220),
            ListTools = tools,
            ActiveTool = tools[0],
            BagLevel = 4,
            BagItems = [],
            Maps = []
        };
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
        public List<string> ListTools { get; set; } = [];
        public string ActiveTool { get; set; } = "";
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
                ListTools = save.ListTools ?? [],
                ActiveTool = save.ActiveTool ?? "",
                BagLevel = save.BagLevel,
                BagItems = save.BagItems ?? [],
                Maps = save.Maps ?? []
            };
        }

        public GameSave ToGameSave()
        {
            string activeTool = ActiveTool ?? "";

            if (tools.Count > 0 && !tools.Contains(activeTool))
                activeTool = tools[0];

            return new GameSave
            {
                Version = Version,
                CurrentMap = string.IsNullOrWhiteSpace(CurrentMap) ? "city" : CurrentMap,
                PlayerLife = PlayerLife,
                PlayerPosition = new Vector2(PlayerX, PlayerY),
                ListTools = tools,
                ActiveTool = activeTool,
                BagLevel = BagLevel,
                BagItems = BagItems ?? [],
                Maps = Maps ?? []
            };
        }
    }
}