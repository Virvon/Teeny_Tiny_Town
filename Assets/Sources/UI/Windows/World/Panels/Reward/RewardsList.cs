using Assets.Sources.Gameplay.PointsCounter;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Reward
{
    public class RewardsList : MonoBehaviour
    {
        private const uint StartGameplayWalletValueReward = 8;
        private const uint StartGameplayWalletValueRewardIncrease = 2;

        [SerializeField] private Button _updateRewardsButton;
        [SerializeField] private TMP_Text _gameplayWalletValueRewardValue;

        private RewardsCreator _rewardsCreator;
        private IUiFactory _uiFactory;
        private Rewarder _rewarder;
        private IPersistentProgressService _persistentProgressService;

        private List<RewardPanel> _rewardPanels;
        private uint _gameplayWalletValueReward;

        [Inject]
        private void Construct(RewardsCreator rewardsCreator, IUiFactory uiFactory, Rewarder rewarder, IPersistentProgressService persistentProgressService)
        {
            _rewardsCreator = rewardsCreator;
            _uiFactory = uiFactory;
            _rewarder = rewarder;
            _persistentProgressService = persistentProgressService;

            _rewardPanels = new();
            _gameplayWalletValueReward = StartGameplayWalletValueReward;

            _rewardsCreator.RewardsCreated += OnRewardsCreated;
            _updateRewardsButton.onClick.AddListener(OnUpdateRewardsButtonClicked);
        }

        public event Action RewardChoosed;

        private void OnDestroy()
        {
            _updateRewardsButton.onClick.RemoveListener(OnUpdateRewardsButtonClicked);
            _rewardsCreator.RewardsCreated -= OnRewardsCreated;

            foreach (RewardPanel rewardPanel in _rewardPanels)
                rewardPanel.Clicked -= OnRewardPanelClicked;
        }

        private async void OnRewardsCreated(IReadOnlyList<RewardType> rewardTypes)
        {
            _gameplayWalletValueRewardValue.text = _gameplayWalletValueReward.ToString();

            foreach (RewardPanel rewardPanel in _rewardPanels)
            {
                rewardPanel.Clicked -= OnRewardPanelClicked;
                Destroy(rewardPanel.gameObject);
            }

            _rewardPanels.Clear();

            foreach (RewardType rewardType in rewardTypes)
            {
                RewardPanel rewardPanel = await _uiFactory.CreateRewardPanel(rewardType, transform);
                _rewardPanels.Add(rewardPanel);
                rewardPanel.Clicked += OnRewardPanelClicked;
            }
        }

        private void OnRewardPanelClicked(RewardPanel rewardPanel)
        {
            _rewarder.Reward(rewardPanel.Type, rewardPanel.RewardCount);
            _persistentProgressService.Progress.Wallet.Give(_gameplayWalletValueReward);
            _gameplayWalletValueReward += StartGameplayWalletValueRewardIncrease;
            RewardChoosed?.Invoke();
        }

        private void OnUpdateRewardsButtonClicked() =>
            _rewardsCreator.CreateRewards();
    }
}
