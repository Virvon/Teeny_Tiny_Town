using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.VisualTheme
{
    public class BuildingVisualTheme : VisualTheme
    {
        [SerializeField] private GameObject _nightLightning;

        private float _nightValue;

        private void Start() =>
            _nightLightning.SetActive(PersistentProgressService.Progress.SettingsData.IsDarkTheme);

        protected override void ChangeTheme()
        {
            ThemeChanger?.Kill();

            ThemeChanger = DOTween.To(() => _nightValue, value => _nightValue = value, PersistentProgressService.Progress.SettingsData.IsDarkTheme ? 1 : 0, AnimationsConfig.ThemeChangingDuration).OnComplete(() =>
            {
                _nightLightning.SetActive(_nightValue == 1);
            });
        }
    }
}
