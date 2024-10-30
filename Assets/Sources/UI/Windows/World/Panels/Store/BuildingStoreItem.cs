using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class BuildingStoreItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _costValue;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Button _buyButton;

        private BuildingType _buildingType;
        private uint _cost;

        public event Action<BuildingType, uint> Buyed;

        private void OnEnable() =>
            _buyButton.onClick.AddListener(OnBuyButtonClicked);

        private void OnDisable() =>
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);

        public void Init(uint cost, BuildingType buildingType)
        {
            _costValue.text = cost.ToString();
            _buildingType = buildingType;
            _cost = cost;
            _name.text = buildingType.ToString();
        }

        private void OnBuyButtonClicked() =>
            Buyed?.Invoke(_buildingType, _cost);

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<BuildingStoreItem>>
        {
        }
    }
}
