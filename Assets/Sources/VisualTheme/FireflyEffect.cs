using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.VisualTheme
{
    public class FireflyEffect : VisualTheme
    {
        [SerializeField] private ParticleSystem _effect;

        private float _nightValue;

        private void Start() =>
            SetActive(PersistentProgressService.Progress.SettingsData.IsDarkTheme);

        protected override void ChangeTheme()
        {
            ThemeChanger?.Kill();

            ThemeChanger = DOTween.To(() => _nightValue, value => _nightValue = value, PersistentProgressService.Progress.SettingsData.IsDarkTheme ? 1 : 0, AnimationsConfig.ThemeChangingDuration).OnComplete(() =>
            {
                SetActive(_nightValue == 1);
            });
        }

        private void SetActive(bool value)
        {
            if (value)
                _effect.Play();
            else
                _effect.Stop();
        }
    }
}
