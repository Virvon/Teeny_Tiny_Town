using Assets.Sources.Sandbox.ActionHandler;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class BuildingButton : ActionHandlerButton
    {
        private BuildingPositionHandler _buildingPositionHandler;

        [Inject]
        private void Construct(BuildingPositionHandler buildingPositionHandler)
        {
            _buildingPositionHandler = buildingPositionHandler;

            _buildingPositionHandler.Entered += OnBuildingPositionHandlerEntered;
            _buildingPositionHandler.Exited += OnBuildingPositionHandlerExited;
        }

        private void OnDestroy()
        {
            _buildingPositionHandler.Entered -= OnBuildingPositionHandlerEntered;
            _buildingPositionHandler.Exited -= OnBuildingPositionHandlerExited;
        }

        private void OnBuildingPositionHandlerExited() =>
            SetActive(false);

        private void OnBuildingPositionHandlerEntered() =>
            SetActive(true);
    }
}
