using Assets.Sources.Data;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "InventorStoreConfig", menuName = "StaticData/GameplayStore/Create new inventory store config", order = 51)]
    public class InventoryStoreConfig : StoreItemConfig
    {
        public override bool NeedToShow(StoreData storeData) =>
            storeData.IsInventoryUnlocked == false;

        public override void Unlock(StoreData storeData) =>
            storeData.IsInventoryUnlocked = true;
    }
}
