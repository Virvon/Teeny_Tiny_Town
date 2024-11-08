using System;
using Cysharp.Threading.Tasks;
using MPUIKIT;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class SandboxPanelElement : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private Color _activeBackgroundColor;
        [SerializeField] private Color _defaultBackgroundColor;
        [SerializeField] private MPImage _background;

        public event Action<SandboxPanelElement> Clicked;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void SetActive(bool value) =>
            _background.color = value ? _activeBackgroundColor : _defaultBackgroundColor;

        public void Init(Sprite icon)
        {
            _icon.sprite = icon;
            _icon.SetNativeSize();
        }

        private void OnButtonClicked() =>
            Clicked?.Invoke(this);

        public class Factory : PlaceholderFactory<string, Transform, UniTask<SandboxPanelElement>>
        {
        }
    }
}
