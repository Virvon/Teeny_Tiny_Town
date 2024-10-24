using Assets.Sources.Gameplay.PointsCounter;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Reward
{
    public class RewardsList : MonoBehaviour
    {
        private RewardsCreator _rewardsCreator;
        private IUiFactory _uiFactory;

        private List<RewardPanel> _rewardPanels;

        [Inject]
        private void Construct(RewardsCreator rewardsCreator, IUiFactory uiFactory)
        {
            _rewardsCreator = rewardsCreator;
            _uiFactory = uiFactory;

            _rewardPanels = new();

            _rewardsCreator.RewardsCreated += OnRewardsCreated;
        }

        public event Action RewardChoosed;

        private void OnDestroy()
        {
            _rewardsCreator.RewardsCreated -= OnRewardsCreated;

            foreach (RewardPanel rewardPanel in _rewardPanels)
                rewardPanel.Clicked -= OnRewardPanelClicked;
        }

        private async void OnRewardsCreated(IReadOnlyList<RewardType> rewardTypes)
        {
            foreach (RewardPanel rewardPanel in _rewardPanels)
            {
                rewardPanel.Clicked -= OnRewardPanelClicked;
                Destroy(rewardPanel.gameObject);
            }

            foreach (RewardType rewardType in rewardTypes)
            {
                RewardPanel rewardPanel = await _uiFactory.CreateRewardPanel(rewardType, transform);
                _rewardPanels.Add(rewardPanel);
                rewardPanel.Clicked += OnRewardPanelClicked;
            }
        }

        private void OnRewardPanelClicked(RewardPanel rewardPanel)
        {
            RewardChoosed?.Invoke();
        }
    }
}
