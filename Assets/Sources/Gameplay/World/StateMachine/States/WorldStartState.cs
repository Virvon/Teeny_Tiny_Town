using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldStartState : IState
    {
        private readonly IWorldData _worldData;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly WorldStateMachine _worldStateMachine;

        public WorldStartState(IWorldData worldData, WindowsSwitcher windowsSwitcher, WorldStateMachine worldStateMachine)
        {
            _worldData = worldData;
            _windowsSwitcher = windowsSwitcher;
            _worldStateMachine = worldStateMachine;
        }

        public UniTask Enter()
        {
            if (_worldData.IsChangingStarted)
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