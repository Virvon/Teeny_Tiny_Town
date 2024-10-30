using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class GainBuyer
    {
        private readonly WorldStateMachine _worldStateMachine;
        private readonly NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;

        public GainBuyer(WorldStateMachine worldStateMachine, NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            _worldStateMachine = worldStateMachine;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
        }

        public void Buy(GainStoreItemType type)
        {
            switch (type)
            {
                case GainStoreItemType.ReplaceItems:
                    _worldStateMachine.Enter<GainBuyingState, GainStoreItemType>(type).Forget();
                    break;
                case GainStoreItemType.Bulldozer:
                    _worldStateMachine.Enter<GainBuyingState, GainStoreItemType>(type).Forget();
                    break;
                case GainStoreItemType.Crane:
                    _nextBuildingForPlacingCreator.ChangeCurrentBuildingForPlacing(BuildingType.Crane);
                    break;
                case GainStoreItemType.Lighthouse:
                    _nextBuildingForPlacingCreator.ChangeCurrentBuildingForPlacing(BuildingType.Lighthouse);
                    break;
            }
        }
    }
}
