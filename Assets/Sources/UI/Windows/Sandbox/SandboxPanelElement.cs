using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class SandboxPanelElement : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;

        public event Action<SandboxPanelElement> Clicked;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void Init( string name)
        {
            _text.text = name;
        }

        private void OnButtonClicked() =>
            Clicked?.Invoke(this);

        public class Factory : PlaceholderFactory<string, Transform, UniTask<SandboxPanelElement>>
        {
        }
    }
}
