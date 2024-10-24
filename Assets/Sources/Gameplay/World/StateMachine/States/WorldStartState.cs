using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldStartState : IState
    {
        private readonly IWorldData _worldData;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;
        private readonly WorldStateMachine _worldStateMachine;

        public WorldStartState(IWorldData worldData, WindowsSwitcher windowsSwitcher, IUiFactory uiFactory, WorldStateMachine worldStateMachine)
        {
            _worldData = worldData;
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
            _worldStateMachine = worldStateMachine;
        }

        public async UniTask Enter()
        {
            if (_worldData.IsChangingStarted)
                _worldStateMachine.Enter<WorldChangingState>().Forget();
            else
                await ShowAdditionalBonusOffer();
        }

        public UniTask Exit() =>
            default;

        private async UniTask ShowAdditionalBonusOffer()
        {
            if (_windowsSwitcher.Contains(WindowType.AdditionalBonusOfferWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.AdditionalBonusOfferWindow);
                _windowsSwitcher.RegisterWindow(WindowType.AdditionalBonusOfferWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.AdditionalBonusOfferWindow);
        }
    }
}
