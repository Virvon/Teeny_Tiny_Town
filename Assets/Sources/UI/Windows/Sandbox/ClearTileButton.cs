using Assets.Sources.Sandbox.ActionHandler;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class ClearTileButton : ActionHandlerButton
    {
        private ClearTilePositionHandler _clearTilePositionHandler;

        [Inject]
        private void Construct(ClearTilePositionHandler clearTilePositionHandler)
        {
            _clearTilePositionHandler = clearTilePositionHandler;

            _clearTilePositionHandler.Entered += OnClearTilePositionHandlerEntered;
            _clearTilePositionHandler.Exited += OnClearTilePositionHandlerExited;
        }

        private void OnDestroy()
        {
            _clearTilePositionHandler.Entered -= OnClearTilePositionHandlerEntered;
            _clearTilePositionHandler.Exited -= OnClearTilePositionHandlerExited;
        }

        private void OnClearTilePositionHandlerExited() =>
            SetActive(false);

        private void OnClearTilePositionHandlerEntered() =>
            SetActive(true);
    }
}
