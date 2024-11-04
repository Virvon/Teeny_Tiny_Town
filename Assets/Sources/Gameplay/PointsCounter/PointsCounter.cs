using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;

namespace Assets.Sources.Gameplay.PointsCounter
{
    public class PointsCounter : IDisposable
    {
        private readonly IWorldData _worldData;
        private readonly IStaticDataService _staticDataService;

        public PointsCounter(IWorldData worldData, IStaticDataService staticDataService)
        {
            _worldData = worldData;
            _staticDataService = staticDataService;

            _worldData.BuildingUpgraded += OnBuildingUpdated;
        }

        public void Dispose() =>
            _worldData.BuildingUpgraded -= OnBuildingUpdated;

        private void OnBuildingUpdated(BuildingType type) =>
            _worldData.PointsData.Give(_staticDataService.GetBuilding<BuildingConfig>(type).PointsRewardForMerge);
    }
}
