using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.VisualTheme
{
    public abstract class VisualTheme : MonoBehaviour
    {
        protected Tween ThemeChanger;

        protected IPersistentProgressService PersistentProgressService { get; private set; }
        protected AnimationsConfig AnimationsConfig { get; private set; }

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            PersistentProgressService = persistentProgressService;
            AnimationsConfig = staticDataService.AnimationsConfig;

            PersistentProgressService.Progress.SettingsData.ThemeChanged += ChangeTheme;
        }

        private void OnDestroy()
        {
            PersistentProgressService.Progress.SettingsData.ThemeChanged -= ChangeTheme;
            ThemeChanger?.Kill();
        }

        protected abstract void ChangeTheme();
    }
}
