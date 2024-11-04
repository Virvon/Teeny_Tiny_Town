using System;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [Serializable]
    public class SandboxGroundConfig
    {
        public SandboxGroundType Type;
        public AssetReference IconAssetReference;
    }
}
