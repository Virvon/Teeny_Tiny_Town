using Assets.Sources.Data.World;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.Reward
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "StaticData/Reward/Create new reward config", order = 51)]
    public class RewardConfig : ScriptableObject
    {
        public RewardType Type;
        public AssetReferenceGameObject AssetReference;
        public AssetReference IconAssetReference;
        public uint ProportionOfLoss;
        public int MinCount;
        public int MaxCount;

        public virtual uint GetRewardCount(IWorldData worldData) =>
            (uint)Random.Range(MinCount, MaxCount);
    }
}