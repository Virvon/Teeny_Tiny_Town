using System;
using Assets.Sources.Data.World;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Reward
{
    public class RewardPanel : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _countValue;

        private IStaticDataService _staticDataService;
        private IWorldData _worldData;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IWorldData worldData)
        {
            _staticDataService = staticDataService;
            _worldData = worldData;
        }

        public RewardType Type { get; private set; }
        public uint RewardCount { get; private set; }

        public event Action<RewardPanel> Clicked;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void Init(Sprite icon, RewardType rewardType)
        {
            _icon.sprite = icon;
            Type = rewardType;

            RewardCount = _staticDataService.GetReward(Type).GetRewardCount(_worldData);

            _countValue.text = RewardCount.ToString();
        }

        private void OnButtonClicked() =>
            Clicked?.Invoke(this);

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<RewardPanel>>
        {
        }
    }
}
