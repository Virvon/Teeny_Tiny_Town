using Assets.Sources.Data;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.GameplayStore
{
    [CreateAssetMenu(fileName = "InfinityMovesStoreConfig", menuName = "StaticData/GameplayStore/Create new infinity moves store config", order = 51)]
    public class InfinityMovesStoreConfig : StoreItemConfig
    {
        public override bool NeedToShow(StoreData storeData) =>
            storeData.IsInventoryUnlocked == false;

        public override void Unlock(StoreData storeData) =>
            storeData.IsInfinityMovesUnlocked = true;
    }
}
