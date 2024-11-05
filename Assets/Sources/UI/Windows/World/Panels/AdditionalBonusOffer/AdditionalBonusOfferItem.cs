using Assets.Sources.Data.World;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.AdditionalBonusOffer
{
    public class AdditionalBonusOfferItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countValue;
        [SerializeField] private TMP_Text _costValue;

        private IStaticDataService _staticDataService;
        private IPersistentProgressService _persistentProgressService;
        private IWorldData _worldData;

        private AdditionalBonusType _type;
        private uint _cost;

        [Inject]
        private void Construct(IPersistentProgressService persistentPRogressService, IStaticDataService staticDataService, IWorldData worldData)
        {
            _staticDataService = staticDataService;
            _persistentProgressService = persistentPRogressService;
            _worldData = worldData;

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
            if (_persistentProgressService.Progress.Wallet.TryGet(_cost))
            {
                _staticDataService.GetAdditionalBonus(_type).Anwenden(_worldData);
                Destroy(gameObject);
            }
        }

        public void Init(AdditionalBonusType type, Sprite icon)
        {
            _type = type;

            AdditionalBonusConfig config = _staticDataService.GetAdditionalBonus(_type);

            _cost = config.Cost;

            _icon.sprite = icon;
            _countValue.text = config.Count.ToString();
            _costValue.text = _cost.ToString();
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<AdditionalBonusOfferItem>>
        {
        }
    }
}
