using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using System;
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

            ChangeCountValue(WorldData.BuldozerItems.ItemsCount);

            _removedBuildingPositionHandler.Entered += OnRemovedBuildingPositionHandlerEntered;
            _removedBuildingPositionHandler.Exited += OnRemovedBuildingPositionHandlerExited;
            WorldData.BuldozerItems.CountChanged += ChangeCountValue;
        }

        private void OnDisable()
        {
            _removedBuildingPositionHandler.Entered -= OnRemovedBuildingPositionHandlerEntered;
            _removedBuildingPositionHandler.Exited -= OnRemovedBuildingPositionHandlerExited;
            WorldData.BuldozerItems.CountChanged -= ChangeCountValue;
        }

        private void OnRemovedBuildingPositionHandlerExited() =>
            SetActive(false);

        private void OnRemovedBuildingPositionHandlerEntered() =>
            SetActive(true);
    }
}
