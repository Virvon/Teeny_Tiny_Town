using Assets.Sources.Gameplay.PointsCounter;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class RewardState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly RewardsCreator _rewardsCreator;

        public RewardState(WindowsSwitcher windowsSwitcher, RewardsCreator rewardsCreator)
        {
            _windowsSwitcher = windowsSwitcher;
            _rewardsCreator = rewardsCreator;
        }

        public UniTask Enter()
        {
            _windowsSwitcher.Switch<RewardWindow>("reward stat");
            _rewardsCreator.CreateRewards();

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
