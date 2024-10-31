using Assets.Sources.Data.World.Currency;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.WorldStore
{
    [CreateAssetMenu(fileName = "LimitedQuantityGainStorItemConfig", menuName = "StaticData/WorldStore/Create new limited quantity gain store item config", order = 51)]
    public class LimitedQuantityGainStoreItemConfig : GainStoreItemConfig
    {
        public uint AvailableCount;

        public override GainStoreItemData GetData() =>
            new LimitedQuantityStoreItemData(Type, Cost, AvailableCount);
    }
}
