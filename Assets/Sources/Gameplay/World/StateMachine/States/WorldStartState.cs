using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldStartState : IState
    {
        private readonly IWorldData _worldData;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly WorldStateMachine _worldStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;

        public WorldStartState(
            IWorldData worldData,
            WindowsSwitcher windowsSwitcher,
            WorldStateMachine worldStateMachine,
            IPersistentProgressService persistentProgressService)
        {
            _worldData = worldData;
            _windowsSwitcher = windowsSwitcher;
            _worldStateMachine = worldStateMachine;
            _persistentProgressService = persistentProgressService;
        }

        public UniTask Enter()
        {
            if (_persistentProgressService.Progress.GameplayMovesCounter.CanMove == false)
                _worldStateMachine.Enter<WaitingState>().Forget();
            else if (_worldData.IsChangingStarted)
                _worldStateMachine.Enter<WorldChangingState>().Forget();
            else
                ShowAdditionalBonusOffer();

            return default;
        }

        public UniTask Exit() =>
            default;

        private void ShowAdditionalBonusOffer()
        {
            _windowsSwitcher.Switch<AdditionalBonusOfferWindow>();
        }
    }
}