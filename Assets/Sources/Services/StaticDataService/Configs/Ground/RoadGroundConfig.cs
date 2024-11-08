using System;
using Assets.Sources.Services.StaticDataService.Configs.Building;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [Serializable]
    public class RoadGroundConfig
    {
        public BuildingType BuildingType;
        public GroundType GroundType;
    }
}
