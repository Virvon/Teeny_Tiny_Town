using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldWallet WorldWallet;
        public WorldData[] WorldDatas;

        public PlayerProgress(WorldData[] worldDatas)
        {
            WorldWallet = new();
            WorldDatas = worldDatas;
        }
    }
}
