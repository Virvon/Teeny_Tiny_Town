using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class StoreData
    {
        public bool IsInventoryUnlocked;
        public bool IsInfinityMovesUnlocked;

        public StoreData()
        {
            IsInventoryUnlocked = false;
            IsInfinityMovesUnlocked = false;
        }
    }
}
