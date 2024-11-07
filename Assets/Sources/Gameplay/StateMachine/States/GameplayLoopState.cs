using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayLoopState : IPayloadState<bool>
    {
        private readonly WorldsList _worldsList;
        private readonly WindowsSwitcher _windowsSwitcher;

        public GameplayLoopState(WorldsList worldsList, WindowsSwitcher windowsSwitcher)
        {
            _worldsList = worldsList;
            _windowsSwitcher = windowsSwitcher;
        }

        public async UniTask Enter(bool startCurrentWorld)
        {
            _windowsSwitcher.HideCurrentWindow();

            if (startCurrentWorld)
                await _worldsList.StartCurrentWorld();
            else
                await _worldsList.StartLastPlayedWorld();
        }

        public UniTask Exit() =>
            default;
    }
}
