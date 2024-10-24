using Assets.Sources.Gameplay.PointsCounter;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class RewardState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;
        private readonly RewardsCreator _rewardsCreator;

        public RewardState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory, RewardsCreator rewardsCreator)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
            _rewardsCreator = rewardsCreator;
        }

        public async UniTask Enter()
        {
            if (_windowsSwitcher.Contains(WindowType.RewardWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.RewardWindow);
                _windowsSwitcher.RegisterWindow(WindowType.RewardWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.RewardWindow);
            _rewardsCreator.CreateRewards();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
