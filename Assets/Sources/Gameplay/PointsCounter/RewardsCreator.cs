using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Data.World;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.Gameplay.PointsCounter
{
    public class RewardsCreator
    {
        private readonly IWorldData _worldData;
        private readonly IStaticDataService _staticDataService;

        public RewardsCreator(IWorldData worldData, IStaticDataService staticDataService)
        {
            _worldData = worldData;
            _staticDataService = staticDataService;
        }

        public event Action<IReadOnlyList<RewardType>> RewardsCreated;

        public void CreateRewards()
        {
            List<RewardType> rewards = new ();
            RewardType[] availableRewards = _staticDataService.GetWorld<WorldConfig>(_worldData.Id).AvailableRewards;

            int rewardVariansCount = GetRewardVariantsCount();

            for (int i = 0; i < rewardVariansCount; i++)
                rewards.Add(GetRewards(availableRewards.Except(rewards).ToArray()));

            RewardsCreated?.Invoke(rewards);
        }

        private int GetRewardVariantsCount()
        {
            WorldConfig worldConfig = _staticDataService.GetWorld<WorldConfig>(_worldData.Id);

            return Random.Range(worldConfig.MinRewardVariantsCount, worldConfig.MaxRewardVariantsCount + 1);
        }

        private RewardType GetRewards(RewardType[] availableRewards)
        {
            RewardConfig[] rewardConfigs = availableRewards
                .Select(rewardType => _staticDataService.GetReward(rewardType))
                .OrderBy(rewardConfig => rewardConfig.ProportionOfLoss)
                .ToArray();

            int proportionsOfLossSum = (int)rewardConfigs.Sum(value => value.ProportionOfLoss);

            int resultChance = Random.Range(0, proportionsOfLossSum) + 1;
            uint chance = 0;

            for (int i = 0; i < rewardConfigs.Length; i++)
            {
                chance += rewardConfigs[i].ProportionOfLoss;

                if (resultChance <= chance)
                    return rewardConfigs[i].Type;
            }

            Debug.LogError("Reward type not founded");
            return RewardType.ReplaceItem;
        }
    }
}
