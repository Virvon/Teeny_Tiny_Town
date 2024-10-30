using Assets.Sources.Data.WorldDatas.Currency;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.WorldStore
{
    [CreateAssetMenu(fileName = "GainStorItemConfig", menuName = "StaticData/WorldStore/Create new gain store item config", order = 51)]
    public class GainStoreItemConfig : ScriptableObject
    {
        public GainStoreItemType Type;
        public AssetReferenceGameObject PanelAssetReference;
        public AssetReference IconAssetReferecne;
        public string Name;
        public uint Cost;

        public virtual GainStoreItemData GetData() =>
            new GainStoreItemData(Type, Cost);
    }
}
