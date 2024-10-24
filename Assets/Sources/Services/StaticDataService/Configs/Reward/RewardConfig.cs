using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.Reward
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "StaticData/Create new reward config", order = 51)]
    public class RewardConfig : ScriptableObject
    {
        public RewardType Type;
        public AssetReferenceGameObject AssetReference;
        public AssetReference IconAssetReference;
        public uint ProportionOfLoss;
    }
}