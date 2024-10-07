using Assets.Sources.Services.StaticDataService.Configs.World;
using System;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [Serializable]
    public class TestGroundConfig
    {
        public TileType TileType;
        public AssetReferenceGameObject AssetReference;
    }
}
