using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "WorldsConfig", menuName = "StaticData/Create new worlds config", order = 51)]
    public class WorldsConfig : ScriptableObject
    {
        public float DistanceBetweenWorlds;
        public Vector3 CurrentWorldPosition;
        public uint[] Goals;
        public uint AvailableMovesCount;
        public string EducationWorldId;
        public AssetReferenceGameObject EducationWorldAssetReference;
    }
}
