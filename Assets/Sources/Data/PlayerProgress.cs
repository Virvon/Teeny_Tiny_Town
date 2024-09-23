using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldWallet WorldWallet;

        public PlayerProgress()
        {
            WorldWallet = new();
        }
    }
}
