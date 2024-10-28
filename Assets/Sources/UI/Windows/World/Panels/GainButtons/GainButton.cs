using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.GainButtons
{
    public class GainButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private MPImage _background;
        [SerializeField] private TMP_Text _countValue;

        private AnimationsConfig _animationsConfig;

        [Inject]
        private void Construct(IWorldData worldData, IStaticDataService staticDataService)
        {
            WorldData = worldData;
            _animationsConfig = staticDataService.AnimationsConfig;
        }

        protected IWorldData WorldData { get; private set; }

        protected void SetActive(bool isActive)
        {
            Color targetColor = isActive ? _animationsConfig.ActiveGainButtonColor : _animationsConfig.DefaultGainButtonColor;

            _icon.color = isActive ? _animationsConfig.ActiveGainButtonIconColor : _animationsConfig.DefaultGainButtonIconColor;
            _background.DOColor(targetColor, _animationsConfig.ChangeGainButtonActiveDuration);
        }

        protected void ChangeCountValue(uint value) =>
            _countValue.text = value.ToString();
    }
}
