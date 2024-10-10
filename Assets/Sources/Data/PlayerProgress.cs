using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    { 
        public WorldData[] WorldDatas;
        //public uint MovesCount;
        public MoveCounterData MoveCounter;

        public PlayerProgress(WorldData[] worldDatas)
        {
            WorldDatas = worldDatas;
            //MovesCount = 0;
            MoveCounter = new();
        }
    }
}
