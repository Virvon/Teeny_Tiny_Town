using Assets.Sources.Sandbox.ActionHandler;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class GroundButton : ActionHandlerButton
    {
        private GroundPositionHandler _groundPositionHandler;

        [Inject]
        private void Construct(GroundPositionHandler groundPositionHandler)
        {
            _groundPositionHandler = groundPositionHandler;

            _groundPositionHandler.Entered += OnGroundPositionHandlerEntered;
            _groundPositionHandler.Exited += OnGroundPositionHandlerExited;
        }

        private void OnDestroy()
        {
            _groundPositionHandler.Entered -= OnGroundPositionHandlerEntered;
            _groundPositionHandler.Exited -= OnGroundPositionHandlerExited;
        }

        private void OnGroundPositionHandlerExited() =>
            SetActive(false);

        private void OnGroundPositionHandlerEntered() =>
            SetActive(true);
    }
}
