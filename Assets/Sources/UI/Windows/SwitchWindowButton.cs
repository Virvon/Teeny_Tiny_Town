using Assets.Sources.Services.StaticDataService.Configs.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class SwitchWindowButton : MonoBehaviour
    {
        [SerializeField] private WindowType _targetWindowType;
        [SerializeField] private Button _button;

        private WindowsSwitcher _windowsSwitcher;

        [Inject]
        private void Construct(WindowsSwitcher windowsSwitcher) =>
            _windowsSwitcher = windowsSwitcher;

        private void OnEnable() =>
            _button.onClick.AddListener(Switch);

        private void OnDisable() =>
            _button.onClick.RemoveListener(Switch);

        private void Switch() =>
            _windowsSwitcher.Switch(_targetWindowType);
    }
}
