using System;
using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine;

namespace Assets.Sources.Gameplay.ExpandingLogic
{
    public class WorldExpander : IDisposable
    {
        private readonly IWorldData _worldData;
        private readonly ExpandingWorldConfig _expandingWorldConfig;
        private readonly IExpandingGameplayMover _expandingGameplayMover;

        public WorldExpander(IStaticDataService staticDataService, IWorldData worldData, IExpandingGameplayMover expandingGameplayMover)
        {
            _worldData = worldData;

            _expandingWorldConfig = staticDataService.GetWorld<ExpandingWorldConfig>(worldData.Id);

            _worldData.BuildingUpgraded += OnBuildingUpdated;
            _expandingGameplayMover = expandingGameplayMover;
        }

        public void Dispose() =>
            _worldData.BuildingUpgraded -= OnBuildingUpdated;

        private void OnBuildingUpdated(BuildingType type)
        {
            Debug.Log("building updated");

            if (_expandingWorldConfig.ContainsExpand(type, out ExpandConfig expandConfig) && expandConfig.ExpandedSize.magnitude > _worldData.Size.magnitude)
                _expandingGameplayMover.ExpandWorld(expandConfig.ExpandedSize);
        }
    }
}
