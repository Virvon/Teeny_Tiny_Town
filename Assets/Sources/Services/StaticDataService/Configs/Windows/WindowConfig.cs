using System;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.Windows
{
    [Serializable]
    public class WindowConfig
    {
        public AssetReferenceGameObject AssetReference;
        public WindowType Type;
    }
}
