using Assets.Sources.Data.WorldDatas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses
{
    public class AdAdditionalBonusPanel : MonoBehaviour
    {
        [SerializeField] private uint _buldozerItemsCount;
        [SerializeField] private uint _replaceItemsCount;
        [SerializeField] private TMP_Text _buldozerItemsCountValue;
        [SerializeField] private TMP_Text _replaceItemsCountValue;
        [SerializeField] private Button _button;

        private IWorldData _worldData;

        [Inject]
        private void Construct(IWorldData worldData)
        {
            _worldData = worldData;

            _buldozerItemsCountValue.text = "+" + _buldozerItemsCount.ToString();
            _replaceItemsCountValue.text = "+" + _replaceItemsCount.ToString();

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
            _worldData.BulldozerItems.AddItems(_buldozerItemsCount);
            _worldData.ReplaceItems.AddItems(_replaceItemsCount);
            Destroy(gameObject);
        }
    }
}