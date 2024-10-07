using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "RoadGroundConfigs", menuName = "StaticData/Create new road ground config", order = 51)]
    public class RoadGroundConfigs : ScriptableObject
    {
       public RoadGroundConfig[] Configs;
    }
}
