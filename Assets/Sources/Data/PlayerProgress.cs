using Assets.Sources.Data.WorldDatas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    { 
        public WorldData[] WorldDatas;
        public MoveCounterData MoveCounter;
        public WorldData CurrentWorldData;
        public bool IsInventoryUnlocked;
        public Wallet Wallet;
        public List<QuestData> Quests;

        public PlayerProgress(WorldData[] worldDatas, List<QuestData> quests)
        {
            WorldDatas = worldDatas;
            Quests = quests;

            MoveCounter = new();
            Wallet = new();

            CurrentWorldData = WorldDatas[0];
            IsInventoryUnlocked = false;
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
