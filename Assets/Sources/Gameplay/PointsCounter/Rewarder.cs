using Assets.Sources.Data.World.Currency;
using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using UnityEngine;

namespace Assets.Sources.Gameplay.PointsCounter
{
    public class Rewarder
    {
        private readonly IWorldData _worldData;
        private readonly NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;
        public Rewarder(IWorldData worldData, NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            _worldData = worldData;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
        }

        public void Reward(RewardType rewardType, uint rewardCount)
        {
            switch (rewardType)
            {
                case RewardType.ReplaceItem:
                    _worldData.ReplaceItems.AddItems(rewardCount);
                    break;
                case RewardType.Bulldozer:
                    _worldData.BulldozerItems.AddItems(rewardCount);
                    break;
                case RewardType.Crane:
                    _nextBuildingForPlacingCreator.ChangeCurrentBuildingForPlacing(BuildingType.Crane);
                    break;
                case RewardType.Lighthouse:
                    _nextBuildingForPlacingCreator.ChangeCurrentBuildingForPlacing(BuildingType.Lighthouse);
                    break;
                case RewardType.WorldWalletValue:
                    AddWorldWalletValue(rewardCount);
                    break;
            }
        }

        private void AddWorldWalletValue(uint count)
        {
            if (_worldData is ICurrencyWorldData currencyWorldData)
                currencyWorldData.WorldWallet.Give(count);
            else
                Debug.Log($"{nameof(_worldData)} is not {typeof(ICurrencyWorldData)}");
        }
    }
}
