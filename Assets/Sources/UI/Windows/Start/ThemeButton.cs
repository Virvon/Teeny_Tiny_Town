using Assets.Sources.Services.PersistentProgress;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start
{
    public class ThemeButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _sunIcon;
        [SerializeField] private Sprite _moonIcon;
        [SerializeField] private Button _button;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;

            ChangeThemeIcon();

            _persistentProgressService.Progress.SettingsData.ThemeChanged += ChangeThemeIcon;
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _persistentProgressService.Progress.SettingsData.ThemeChanged -= ChangeThemeIcon;
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked() =>
            _persistentProgressService.Progress.SettingsData.ChangeTheme(_persistentProgressService.Progress.SettingsData.IsDarkTheme == false);

        private void ChangeThemeIcon() =>
            _icon.sprite = _persistentProgressService.Progress.SettingsData.IsDarkTheme ? _moonIcon : _sunIcon;
    }
}
