using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.Gameplay.VisualTheme
{
    public class LightingVisualTheme : VisualTheme
    {
        [SerializeField] private Vector3 _sunThemeRotation;
        [SerializeField] private Vector3 _darkThemeRotation;

        private void Start()
        {
            Vector3 rotation = PersistentProgressService.Progress.IsDarkTheme ? _darkThemeRotation : _sunThemeRotation;

            transform.rotation = Quaternion.Euler(rotation);
        }

        protected override void ChangeTheme()
        {
            ThemeChanger?.Kill();

            Vector3 targetRotation = PersistentProgressService.Progress.IsDarkTheme ? _darkThemeRotation : _sunThemeRotation;
            ThemeChanger = transform.DORotateQuaternion(Quaternion.Euler(targetRotation), AnimationsConfig.ThemeChangingDuration);
        }
    }
}
