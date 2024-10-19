using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "AnimationsConfig", menuName = "StaticData/Create new animations config", order = 51)]
    public class AnimationsConfig : ScriptableObject
    {
        public Vector3 BuildingJumpDestroyOffset;
        public float BuildingJumpDestroyPower;
        public float BuildingJumpDestroyDuration;
        public float TileUpdatingDuration;
        public float BuildingPutMaxScale;
        public float BuildingPutMinScale;
        public float BuildingPutDuration;
        public float BuildingBlinkingScale;
        public float BuildingBlinkingDuration;
    }
}
