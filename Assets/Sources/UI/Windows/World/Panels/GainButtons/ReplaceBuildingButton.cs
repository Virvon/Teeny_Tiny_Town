using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Sandbox.ActionHandler;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.GainButtons
{
    public class ReplaceBuildingButton : GainButton
    {
        private ReplacedBuildingPositionHandler _replacedBuildingPositionHandler;

        [Inject]
        private void Construct(ReplacedBuildingPositionHandler replacedBuildingPositionHandler)
        {
            _replacedBuildingPositionHandler = replacedBuildingPositionHandler;

            ChangeCountValue(WorldData.ReplaceItems.Count);

            _replacedBuildingPositionHandler.Entered += OnReplaceBuildingPositionHandlerEntered;
            _replacedBuildingPositionHandler.Exited += OnReplaceBuildingPositionHandlerExited;
            WorldData.ReplaceItems.CountChanged += ChangeCountValue;
        }

        private void OnDisable()
        {
            _replacedBuildingPositionHandler.Entered -= OnReplaceBuildingPositionHandlerEntered;
            _replacedBuildingPositionHandler.Exited -= OnReplaceBuildingPositionHandlerExited;
            WorldData.BulldozerItems.CountChanged -= ChangeCountValue;
        }

        private void OnReplaceBuildingPositionHandlerExited() =>
            SetActive(false);

        private void OnReplaceBuildingPositionHandlerEntered() =>
            SetActive(true);
    }
}
