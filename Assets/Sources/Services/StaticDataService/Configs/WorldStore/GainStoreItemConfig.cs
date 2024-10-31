using Assets.Sources.Data.World.Currency;
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
        public float CostCoefficient;

        public virtual GainStoreItemData GetData() =>
            new GainStoreItemData(Type, Cost);

        public uint GetCost(uint n) =>
            (uint)(Cost * Mathf.Pow(CostCoefficient, n - 1));

        public uint GetCostsSum(uint b, uint n)
        {
            uint sum = 0;

            for (uint i = b + 1; i <= n; i++)
                sum += GetCost(i);

            return sum;
        }
    }
}
