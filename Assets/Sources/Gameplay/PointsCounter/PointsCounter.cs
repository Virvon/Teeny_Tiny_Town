using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;

namespace Assets.Sources.Gameplay.PointsCounter
{
    public class PointsCounter
    {
        private readonly IWorldData _worldData;
        private readonly IStaticDataService _staticDataService;

        public PointsCounter(IWorldData worldData, IStaticDataService staticDataService)
        {
            _worldData = worldData;

            _worldData.BuildingUpgraded += OnBuildingUpdated;
            _staticDataService = staticDataService;
        }

        ~PointsCounter()=>
            _worldData.BuildingUpgraded -= OnBuildingUpdated;

        private void OnBuildingUpdated(BuildingType type) =>
            _worldData.PointsData.Give(_staticDataService.GetBuilding<BuildingConfig>(type).PointsRewardForMerge);
    }
}
