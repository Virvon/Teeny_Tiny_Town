using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start
{
    public class StoreItem : MonoBehaviour
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private TMP_Text _costValue;

        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;

            _costValue.text = _staticDataService.StoreItemConfig.Cost.ToString();

            if (_persistentProgressService.Progress.IsInventoryUnlocked)
                Destroy(gameObject);
        }

        private void OnEnable() =>
            _buyButton.onClick.AddListener(OnBuyButtonClicked);

        private void OnDisable() =>
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);

        private void OnBuyButtonClicked()
        {
            if (_persistentProgressService.Progress.Wallet.TryGet(_staticDataService.StoreItemConfig.Cost))
            {
                _persistentProgressService.Progress.IsInventoryUnlocked = true;
                Destroy(gameObject);
            }
        }
    }
}
