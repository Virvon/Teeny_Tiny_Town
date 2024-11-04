using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using MPUIKIT;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI
{
    public class ActionHandlerButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private MPImage _background;

        private AnimationsConfig _animationsConfig;

        [Inject]
        private void Construct(IStaticDataService staticDataService) =>
            _animationsConfig = staticDataService.AnimationsConfig;

        protected void SetActive(bool isActive)
        {
            Color targetColor = isActive ? _animationsConfig.ActiveGainButtonColor : _animationsConfig.DefaultGainButtonColor;

            _icon.color = isActive ? _animationsConfig.ActiveActionHandlerButtonIconColor : _animationsConfig.DefaultActionHandlerButtonIconColor;
            _background.DOColor(targetColor, _animationsConfig.ChangeGainButtonActiveDuration);
        }
    }
}
