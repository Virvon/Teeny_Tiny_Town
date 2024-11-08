using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.GainButtons
{
    public class RemoveBuildingButton : GainButton
    {
        private RemovedBuildingPositionHandler _removedBuildingPositionHandler;

        [Inject]
        private void Construct(RemovedBuildingPositionHandler removedBuildingPositionHandler)
        {
            _removedBuildingPositionHandler = removedBuildingPositionHandler;

            ChangeCountValue(WorldData.BulldozerItems.Count);

            _removedBuildingPositionHandler.Entered += OnRemovedBuildingPositionHandlerEntered;
            _removedBuildingPositionHandler.Exited += OnRemovedBuildingPositionHandlerExited;
            WorldData.BulldozerItems.CountChanged += ChangeCountValue;
        }

        private void OnDestroy()
        {
            _removedBuildingPositionHandler.Entered -= OnRemovedBuildingPositionHandlerEntered;
            _removedBuildingPositionHandler.Exited -= OnRemovedBuildingPositionHandlerExited;
            WorldData.BulldozerItems.CountChanged -= ChangeCountValue;
        }

        private void OnRemovedBuildingPositionHandlerExited() =>
            SetActive(false);

        private void OnRemovedBuildingPositionHandlerEntered() =>
            SetActive(true);
    }
}
