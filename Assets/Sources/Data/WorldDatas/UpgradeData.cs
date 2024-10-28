using System;

namespace Assets.Sources.Data.WorldDatas
{
    public partial class WorldData
    {
        [Serializable]
        public class UpgradeData
        {
            public uint ItemsCount;

            public UpgradeData() =>
                ItemsCount = 0;

            public event Action<uint> CountChanged;

            public void AddItems(uint count)
            {
                ItemsCount += count;

                CountChanged?.Invoke(ItemsCount);
            }

            public bool TryGet()
            {
                if (ItemsCount == 0)
                    return false;

                ItemsCount--;
                CountChanged?.Invoke(ItemsCount);

                return true;
            }
        }
    }
}