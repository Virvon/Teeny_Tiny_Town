using System;

namespace Assets.Sources.Data.World
{
    [Serializable]
    public class UpgradeData
    {
        public uint Count;

        public UpgradeData() =>
            Count = 0;

        public event Action<uint> CountChanged;

        public void AddItems(uint count)
        {
            Count += count;

            CountChanged?.Invoke(Count);
        }

        public bool TryGet()
        {
            if (Count == 0)
                return false;

            Count--;
            CountChanged?.Invoke(Count);

            return true;
        }

        public void SetItemsCount(uint count)
        {
            Count = count;
            CountChanged?.Invoke(Count);
        }
    }
}