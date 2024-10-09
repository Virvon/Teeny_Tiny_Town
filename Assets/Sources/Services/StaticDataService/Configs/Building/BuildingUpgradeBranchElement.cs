using Assets.Sources.Gameplay.World.WorldInfrastructure;
using System;

namespace Assets.Sources.Services.StaticDataService.Configs.Building
{
    [Serializable]
    public class BuildingUpgradeBranchElement
    {
        public BuildingType BuildingType;
        public BuildingType NextBuildingType;
    }
}