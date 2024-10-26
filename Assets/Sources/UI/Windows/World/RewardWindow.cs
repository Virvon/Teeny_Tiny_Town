using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.UI.Windows.World.Panels.Reward;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World
{
    public class RewardWindow : BluredBackgroundWindow
    {
        [SerializeField] private RewardsList _rewardsList;

        private WorldStateMachine _worldStateMachine;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine)
        {
            _worldStateMachine = worldStateMachine;

            _rewardsList.RewardChoosed += OnRewardChoosed;
        }

        private void OnDisable() =>
            _rewardsList.RewardChoosed -= OnRewardChoosed;

        private void OnRewardChoosed() =>
            _worldStateMachine.Enter<WorldStartState>().Forget();
    }
}
