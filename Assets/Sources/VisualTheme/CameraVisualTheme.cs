using System;
using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Sources.VisualTheme
{
    public class CameraVisualTheme : VisualTheme
    {
        [SerializeField] private Volume _themeVolume;

        private void Start()
        {
            int value = PersistentProgressService.Progress.SettingsData.IsDarkTheme ? 1 : 0;

            _themeVolume.weight = value;
        }

        protected override void ChangeTheme()
        {
            ThemeChanger?.Kill();

            int targetValue = PersistentProgressService.Progress.SettingsData.IsDarkTheme ? 1 : 0;
            ThemeChanger = DOTween.To(() => _themeVolume.weight, value => _themeVolume.weight = value, targetValue, AnimationsConfig.ThemeChangingDuration);
        }
    }
}
