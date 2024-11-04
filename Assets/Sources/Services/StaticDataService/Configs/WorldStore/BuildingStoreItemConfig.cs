using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.WorldStore
{
    [Serializable]
    public class BuildingStoreItemConfig
    {
        public BuildingType BuildingType;
        public AssetReference IconAssetReference;
        public uint Cost;
        public float CostCoefficient;

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
