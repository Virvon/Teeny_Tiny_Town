using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine;

namespace Assets.Sources.Gameplay.ExpandingLogic
{
    public class WorldExpander
    {
        private readonly IExpandingWorldData _expandingWorldData;
        private readonly ExpandingWorldConfig _expandingWorldConfig;
        private readonly IExpandingGameplayMover _expandingGameplayMover;

        public WorldExpander(IStaticDataService staticDataService, IExpandingWorldData expandingWorldData, IExpandingGameplayMover expandingGameplayMover)
        {
            _expandingWorldData = expandingWorldData;

            _expandingWorldConfig = staticDataService.GetWorld<ExpandingWorldConfig>(expandingWorldData.Id);

            _expandingWorldData.BuildingUpdated += OnBuildingUpdated;
            _expandingGameplayMover = expandingGameplayMover;
        }

        ~WorldExpander()
        {
            _expandingWorldData.BuildingUpdated -= OnBuildingUpdated;
        }

        private void OnBuildingUpdated(BuildingType type)
        {
            if (_expandingWorldConfig.ContainsExpand(type, out ExpandConfig expandConfig) && expandConfig.ExpandedSize.magnitude > new Vector2(_expandingWorldData.Length, _expandingWorldData.Width).magnitude)
                _expandingGameplayMover.ExpandWorld((uint)expandConfig.ExpandedSize.x, (uint)expandConfig.ExpandedSize.y);
        }
    }
}
