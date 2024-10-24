using Cysharp.Threading.Tasks;
using System;
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

        public event Action<RewardPanel> Clicked;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void Init(Sprite icon) =>
            _icon.sprite = icon;

        private void OnButtonClicked() =>
            Clicked?.Invoke(this);

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<RewardPanel>>
        {
        }
    }
}
