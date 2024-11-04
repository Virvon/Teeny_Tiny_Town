﻿using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using System;

namespace Assets.Sources.Data.World.Currency
{
    public class GainStoreItemData
    {
        public GainStoreItemType Type;
        public uint BuyingCount;


        public GainStoreItemData(GainStoreItemType type)
        {
            Type = type;

            BuyingCount = 0;
        }

        public event Action BuyingCountChanged;

        public virtual void ChangeBuyingCount(uint count)
        {
            BuyingCount += count;
            BuyingCountChanged?.Invoke();
        }
    }
}
