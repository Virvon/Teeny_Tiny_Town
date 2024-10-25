using Assets.Sources.Data;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{

    public abstract class StoreItemConfig : ScriptableObject
    {
        public GameplayStoreItemType Type;
        public uint Cost;

        public abstract void Unlock(StoreData storeData);
        public abstract bool NeedToShow(StoreData storeData);
    }
}
