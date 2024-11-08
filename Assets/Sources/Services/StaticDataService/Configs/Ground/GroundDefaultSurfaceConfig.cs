using System;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [Serializable]
    public class GroundDefaultSurfaceConfig
    {
        public TileType TileType;
        public AssetReferenceGameObject AssetReference;
    }
}
