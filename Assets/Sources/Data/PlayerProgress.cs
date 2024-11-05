using Assets.Sources.Data.Sandbox;
using Assets.Sources.Data.World;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    { 
        public WorldData[] WorldDatas;
        public StoreData StoreData;
        public Wallet Wallet;
        public List<QuestData> Quests;
        public GameplayMovesCounterData GameplayMovesCounter;
        public SandboxData SandboxData;
        public BuildingData[] BuildingDatas;
        public bool IsEducationCompleted;
        public SettingsData SettingsData;
        public IWorldData LastPlayedWorldData;

        public PlayerProgress(
            WorldData[] worldDatas,
            List<QuestData> quests,
            uint startRemainingMoveCount,
            Vector2Int sandboxSize,
            BuildingType[] allBuildings)
        {
            WorldDatas = worldDatas;
            Quests = quests;

            StoreData = new();
            Wallet = new();
            GameplayMovesCounter = new(startRemainingMoveCount, StoreData);
            SandboxData = new(sandboxSize);
            SettingsData = new();

            LastPlayedWorldData = WorldDatas[0];
            IsEducationCompleted = true;

            BuildingDatas = new BuildingData[allBuildings.Length];

            for(int i = 0; i < allBuildings.Length; i++)
                BuildingDatas[i] = new BuildingData(allBuildings[i]);
        }

        public IWorldData GetNextWorldData(IWorldData currentWorldData)
        {
            int curentWorldDataIndex = Array.IndexOf(WorldDatas, currentWorldData);

            return curentWorldDataIndex >= WorldDatas.Length - 1 ? WorldDatas[0] : WorldDatas[curentWorldDataIndex + 1];
        }

        public IWorldData GetPreviousWorldData(IWorldData currentWorldData)
        {
            int curentWorldDataIndex = Array.IndexOf(WorldDatas, currentWorldData);

            return curentWorldDataIndex <= 0 ? WorldDatas[WorldDatas.Length - 1] : WorldDatas[curentWorldDataIndex - 1];
        }

        public IWorldData ChangeWorldData(string id)
        {
            LastPlayedWorldData = WorldDatas.First(data => data.Id == id);

            return LastPlayedWorldData;
        }

        public QuestData GetQuest(string id) =>
            Quests.First(data => data.Id == id);

        public void AddBuildingToCollection(BuildingType type) =>
            BuildingDatas.First(data => data.Type == type).Count++;
    }
}
