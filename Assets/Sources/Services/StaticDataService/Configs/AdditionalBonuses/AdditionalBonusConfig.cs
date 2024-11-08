using Assets.Sources.Data.World;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses
{
    public abstract class AdditionalBonusConfig : ScriptableObject
    {
        public AssetReferenceGameObject PanelAssetReference;
        public AssetReference IconAssetReference;
        public uint Count;
        public uint Cost;

        public abstract AdditionalBonusType Type { get; }

        public abstract void ApplyBonus(IWorldData worldData);
    }
}