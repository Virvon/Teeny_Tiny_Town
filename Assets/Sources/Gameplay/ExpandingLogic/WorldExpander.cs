﻿using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;

namespace Assets.Sources.Gameplay.ExpandingLogic
{
    public class WorldExpander
    {
        private readonly IWorldData _worldData;
        private readonly ExpandingWorldConfig _expandingWorldConfig;
        private readonly IExpandingGameplayMover _expandingGameplayMover;

        public WorldExpander(IStaticDataService staticDataService, IWorldData worldData, IExpandingGameplayMover expandingGameplayMover)
        {
            _worldData = worldData;

            _expandingWorldConfig = staticDataService.GetWorld<ExpandingWorldConfig>(worldData.Id);

            _worldData.BuildingUpdated += OnBuildingUpdated;
            _expandingGameplayMover = expandingGameplayMover;
        }

        ~WorldExpander()
        {
            _worldData.BuildingUpdated -= OnBuildingUpdated;
        }

        private void OnBuildingUpdated(BuildingType type)
        {
            if (_expandingWorldConfig.ContainsExpand(type, out ExpandConfig expandConfig) && expandConfig.ExpandedSize.magnitude > _worldData.Size.magnitude)
                _expandingGameplayMover.ExpandWorld(expandConfig.ExpandedSize);
        }
    }
}
