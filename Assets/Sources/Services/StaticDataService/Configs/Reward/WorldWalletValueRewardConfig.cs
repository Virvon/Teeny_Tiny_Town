using Assets.Sources.Data.World;
using Assets.Sources.Data.World.Currency;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Reward
{
    [CreateAssetMenu(fileName = "WolrWalletValueRewardConfig", menuName = "StaticData/Reward/Create new world wallet value config", order = 51)]
    public class WorldWalletValueRewardConfig : RewardConfig
    {
        public float Multiplier;
        public uint MinReward;

        public override uint GetRewardCount(IWorldData worldData)
        {
            if (worldData is ICurrencyWorldData currencyWorldData)
            {
                uint reward = (uint)(Random.Range(MinCount, MaxCount + 1) * currencyWorldData.WorldWallet.Value * Multiplier);
                reward = reward < MinReward? MinReward : reward;

                return reward;
            }    
            else
            {
                Debug.LogError($"{nameof(worldData)} is not {typeof(ICurrencyWorldData)}");

                return 0;
            }
        }
    }
}