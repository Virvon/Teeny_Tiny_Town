using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "WorldsConfig", menuName = "StaticData/Create new worlds config", order = 51)]
    public class WorldsConfig : ScriptableObject
    {
        public float DistanceBetweenWorlds;
        public WorldConfig[] Configs;
    }
}
