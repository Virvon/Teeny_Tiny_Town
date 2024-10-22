using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class NextBuildingPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nextBuildingValue;

        private NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;

        [Inject]
        private void Construct(NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;

            _nextBuildingForPlacingCreator.DataChanged += OnBuildingForPlacingDataChanged;

            OnBuildingForPlacingDataChanged(_nextBuildingForPlacingCreator.BuildingsForPlacingData);
        }

        private void OnDestroy() =>
            _nextBuildingForPlacingCreator.DataChanged -= OnBuildingForPlacingDataChanged;

        private void OnBuildingForPlacingDataChanged(BuildingsForPlacingData data)
        {
            _nextBuildingValue.text = data.NextBuildingType.ToString();
        }
    }
}
