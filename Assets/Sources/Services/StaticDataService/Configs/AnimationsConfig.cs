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
        public AnimationCurve BuildingShakeCurve;
        public int BuildingShakesCount;
        public float BuildingShakesDuration;
        public Vector3 BuildingShakeOffset;

        public float WindowOpeningStateDuration;

        public float WorldRotateDuration;
        public float WorldRotateToStarDuration;
        public float WorldSimpleRotateDuration;
        public float WorldMoveDuration;

        public float CameraMoveDuration;

        public Color ActiveGainButtonColor;
        public Color DefaultGainButtonColor;
        public Color ActiveGainButtonIconColor;
        public Color DefaultGainButtonIconColor;
        public float ChangeGainButtonActiveDuration;

        public float BuildingShakeTweenDuration => BuildingShakesDuration / BuildingShakesCount;
    }
}
