using Assets.Sources.Data;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.GameplayStore
{
    public abstract class StoreItemConfig : ScriptableObject
    {
        public GameplayStoreItemType Type;
        public uint Cost;

        public abstract void Unlock(StoreData storeData);
        public abstract bool NeedToShow(StoreData storeData);
    }
}
