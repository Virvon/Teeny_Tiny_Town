using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.Camera
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "StaticData/Create new camera config", order = 51)]
    public class GameplayCameraConfig : ScriptableObject
    {
        public GameplayCameraType Type;
        public AssetReferenceGameObject AssetReference;
    }
}