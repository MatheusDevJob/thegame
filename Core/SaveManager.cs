using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace thegame.Core;

public static class SaveManager
{
    private const int CurrentVersion = 1;
    private const string SaveFileName = "save_02.json";
    private const int EquipableSlotCount = 8;

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
            EquipableIds =
            [
                "AxeTool",
                "PickaxeTool",
                "ShovelTool",
                "",
                "",
                "",
                "",
                ""
            ],
            ActiveEquipe = "",
            BagLevel = 4,
            BagItems = [
                new ItemStackSave{
                    ItemId= "AxeTool",
                    Amount= 1
                },
                new ItemStackSave{
                    ItemId= "PickaxeTool",
                    Amount= 1
                },
                new ItemStackSave  {
                    ItemId= "ShovelTool",
                    Amount= 1
                },
            ],
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

    private static List<string> NormalizeEquipableIds(List<string> equipableIds)
    {
        List<string> normalized = equipableIds ?? [];

        while (normalized.Count < EquipableSlotCount)
            normalized.Add("");

        if (normalized.Count > EquipableSlotCount)
            normalized = normalized.GetRange(0, EquipableSlotCount);

        return normalized;
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
        public List<string> EquipableIds { get; set; } = [];
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
                EquipableIds = NormalizeEquipableIds(save.EquipableIds),
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
                EquipableIds = NormalizeEquipableIds(EquipableIds),
                ActiveEquipe = ActiveEquipe ?? "",
                BagLevel = BagLevel <= 0 ? 4 : BagLevel,
                BagItems = BagItems ?? [],
                Maps = Maps ?? []
            };
        }
    }
}