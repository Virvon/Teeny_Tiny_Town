using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    { 
        public WorldData[] WorldDatas;

        public PlayerProgress(WorldData[] worldDatas)
        {
            WorldDatas = worldDatas;
        }
    }
}
