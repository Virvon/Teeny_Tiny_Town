using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    { 
        public WorldData[] WorldDatas;
        public MoveCounterData MoveCounter;
        public WorldData CurrentWorldData;

        public PlayerProgress(WorldData[] worldDatas)
        {
            WorldDatas = worldDatas;
            MoveCounter = new();

            CurrentWorldData = WorldDatas[0];
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
    }
}
