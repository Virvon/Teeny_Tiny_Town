using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Services.Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Education
{
    public class ContinueEducationPanel : EducationPanel
    {
        [SerializeField] private Button _continueButton;

        [Inject]
        private void Construct(IInputService inputService, ActionHandlerStateMachine actionHandlerStateMachine, MarkersVisibility markersVisibility)
        {
            InputService = inputService;
            ActionHandlerStateMachine = actionHandlerStateMachine;
            MarkersVisibility = markersVisibility;

            _continueButton.onClick.AddListener(OnHandled);
        }

        public IInputService InputService { get; private set; }
        public ActionHandlerStateMachine ActionHandlerStateMachine { get; private set; }
        public MarkersVisibility MarkersVisibility { get; private set; }

        private void OnDestroy() =>
            _continueButton.onClick.RemoveListener(OnHandled);

        public override void Open()
        {
            base.Open();
            MarkersVisibility.ChangeAllowedVisibility(false);
            InputService.SetEnabled(false);
            ActionHandlerStateMachine.SetActive(false);
        }
    }
}
