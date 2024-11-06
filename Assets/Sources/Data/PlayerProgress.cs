using Assets.Sources.Data.Sandbox;
using Assets.Sources.Data.World;
using Assets.Sources.Data.World.Currency;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    { 
        public List<WorldData> WorldDatas;
        public List<CurrencyWorldData> CurrencyWorldDatas;
        public List<ExpandingWorldData> ExpandingWorldDatas;
        public StoreData StoreData;
        public Wallet Wallet;
        public List<QuestData> Quests;
        public GameplayMovesCounterData GameplayMovesCounter;
        public SandboxData SandboxData;
        public BuildingData[] BuildingDatas;
        public bool IsEducationCompleted;
        public SettingsData SettingsData;
        public string LastPlayedWorldDataId;

        public PlayerProgress(
            WorldData[] worldDatas,
            List<QuestData> quests,
            uint startRemainingMoveCount,
            Vector2Int sandboxSize,
            BuildingType[] allBuildings,
            string startWorldId)
        {
            Quests = quests;
            StoreData = new();
            Wallet = new();
            GameplayMovesCounter = new(startRemainingMoveCount, StoreData);
            SandboxData = new(sandboxSize);
            SettingsData = new();
            LastPlayedWorldDataId = startWorldId;
            IsEducationCompleted = true;

            BuildingDatas = new BuildingData[allBuildings.Length];

            for(int i = 0; i < allBuildings.Length; i++)
                BuildingDatas[i] = new BuildingData(allBuildings[i]);

            WorldDatas = new();
            CurrencyWorldDatas = new();
            ExpandingWorldDatas = new();

            foreach(WorldData worldData in worldDatas)
            {
                if (worldData is CurrencyWorldData)
                    CurrencyWorldDatas.Add((CurrencyWorldData)worldData);
                else if (worldData is ExpandingWorldData)
                    ExpandingWorldDatas.Add((ExpandingWorldData)worldData);
                else
                    WorldDatas.Add(worldData);
            }
        }

        public QuestData GetQuest(string id) =>
            Quests.First(data => data.Id == id);

        public void AddBuildingToCollection(BuildingType type) =>
            BuildingDatas.First(data => data.Type == type).Count++;

        public WorldData GetWorldData(string id)
        {
            WorldData worldData = WorldDatas.FirstOrDefault(data => data.Id == id);

            if(worldData == null)
                worldData = CurrencyWorldDatas.FirstOrDefault(data => data.Id == id);

            if(worldData == null)
                worldData = ExpandingWorldDatas.FirstOrDefault(data => data.Id == id);

            if (worldData == null)
                Debug.LogError(nameof(worldData) + " is not founded");

            return worldData;
        }
    }
}
