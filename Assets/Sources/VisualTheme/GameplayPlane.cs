using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.VisualTheme
{
    public class GameplayPlane : VisualTheme
    {
        private const string ColorValue = "_Color";

        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _nightColor;
        [SerializeField] private Color _sunColor;

        private Color Color => PersistentProgressService.Progress.SettingsData.IsDarkTheme ? _nightColor : _sunColor;

        private void Start() =>
            _renderer.material.SetColor(ColorValue, Color);

        protected override void ChangeTheme()
        {
            ThemeChanger?.Kill();

            ThemeChanger = _renderer.material.DOColor(Color, AnimationsConfig.ThemeChangingDuration);
        }

        public class Factory : PlaceholderFactory<string, UniTask<GameplayPlane>>
        {
        }
    }
}