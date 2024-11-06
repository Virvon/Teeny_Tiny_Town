﻿using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using System;

namespace Assets.Sources.Data.World.Currency
{
    [Serializable]
    public class GainStoreItemData
    {
        public GainStoreItemType Type;
        public uint BuyingCount;
        public uint RemainingCount;
        public bool IsLimited;

        public GainStoreItemData(GainStoreItemType type)
        {
            Type = type;

            BuyingCount = 0;
            IsLimited = false;
        }

        public GainStoreItemData(GainStoreItemType type, uint startCount)
        {
            Type = type;
            RemainingCount = startCount;

            IsLimited = true;
        }

        public event Action BuyingCountChanged;

        public virtual void ChangeBuyingCount(uint count)
        {
            RemainingCount = RemainingCount < count ? 0 : RemainingCount - count;
            BuyingCount += count;
            BuyingCountChanged?.Invoke();
        }
    }
}
