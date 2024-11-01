using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using DG.Tweening;
using Zenject;

namespace Assets.Sources.UI.Windows.Education
{
    public class MergedBuildingPanel : EducationPanel
    {
        private IWorldData _worldData;
        private IInputService _inptuService;
        private ActionHandlerStateMachine _actionHandlerStateMachine;
        private MarkersVisibility _markersVisibility;

        [Inject]
        private void Construct(IWorldData worldData, IInputService inputService, ActionHandlerStateMachine actionHandlerStateMachine, MarkersVisibility markersVisibility)
        {
            _worldData = worldData;
            _inptuService = inputService;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _markersVisibility = markersVisibility;
        }

        public override void Open()
        {
            base.Open();
            _markersVisibility.ChangeAllowedVisibility(true);
            _inptuService.SetEnabled(true);
            _actionHandlerStateMachine.SetActive(true);
            _worldData.BuildingUpgraded += OnBuildingUpgraded;
        }

        public override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _worldData.BuildingUpgraded -= OnBuildingUpgraded;
        }

        private void OnBuildingUpgraded(BuildingType obj) =>
            OnHandled();
    }
}
