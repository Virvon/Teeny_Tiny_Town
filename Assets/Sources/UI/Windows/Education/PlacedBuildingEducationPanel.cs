using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Services.Input;
using DG.Tweening;
using System;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Education
{
    public class PlacedBuildingEducationPanel : EducationPanel
    {
        private WorldRepresentationChanger _worldRepresentationChanger;
        private IInputService _inptuService;
        private ActionHandlerStateMachine _actionHandlerStateMachine;
        private MarkersVisibility _markersVisibility;

        [Inject]
        private void Construct(WorldRepresentationChanger worldRepresentationChanger, IInputService inputService, ActionHandlerStateMachine actionHandlerStateMachine, MarkersVisibility markersVisibility)
        {
            _worldRepresentationChanger = worldRepresentationChanger;
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
            _worldRepresentationChanger.GameplayMoved += OnHandled;
        }

        public override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _worldRepresentationChanger.GameplayMoved -= OnHandled;
        }
    }
}
