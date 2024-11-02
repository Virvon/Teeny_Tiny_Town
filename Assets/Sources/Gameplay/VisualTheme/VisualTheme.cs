using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.VisualTheme
{
    public abstract class VisualTheme : MonoBehaviour
    {
        protected Tween ThemeChanger;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            PersistentProgressService = persistentProgressService;
            AnimationsConfig = staticDataService.AnimationsConfig;

            PersistentProgressService.Progress.ThemeChanged += ChangeTheme;
        }

        protected IPersistentProgressService PersistentProgressService { get; private set; }
        protected AnimationsConfig AnimationsConfig { get; private set; }

        private void OnDestroy()
        {
            PersistentProgressService.Progress.ThemeChanged -= ChangeTheme;
            ThemeChanger?.Kill();
        }

        protected abstract void ChangeTheme();
    }
}
