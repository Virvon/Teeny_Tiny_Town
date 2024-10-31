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
        
        public WorldData CurrentWorldData;
        public StoreData StoreData;
        public Wallet Wallet;
        public List<QuestData> Quests;
        public GameplayMovesCounterData GameplayMovesCounter;
        public SandboxData SandboxData;
        public BuildingData[] BuildingDatas;

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

            CurrentWorldData = WorldDatas[0];

            BuildingDatas = new BuildingData[allBuildings.Length];

            for(int i = 0; i < allBuildings.Length; i++)
                BuildingDatas[i] = new BuildingData(allBuildings[i]);
        }

        public WorldData GetNextWorldData()
        {
            int curentWorldDataIndex = Array.IndexOf(WorldDatas, CurrentWorldData);

            CurrentWorldData = curentWorldDataIndex >= WorldDatas.Length - 1 ? WorldDatas[0] : WorldDatas[curentWorldDataIndex + 1];

            return CurrentWorldData;
        }

        public WorldData GetPreviousWorldData()
        {
            int curentWorldDataIndex = Array.IndexOf(WorldDatas, CurrentWorldData);

            CurrentWorldData = curentWorldDataIndex <= 0 ? WorldDatas[WorldDatas.Length - 1] : WorldDatas[curentWorldDataIndex - 1];

            return CurrentWorldData;
        }

        public QuestData GetQuest(string id) =>
            Quests.First(data => data.Id == id);
    }
}
