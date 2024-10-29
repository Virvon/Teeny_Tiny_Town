using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers
{
    public class BuildingMarker : MonoBehaviour
    {
        private IWorldFactory _worldFactory;
        private NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;

        private BuildingRepresentation _building;
        private bool _isHided;

        [Inject]
        private void Construct(IWorldFactory worldFactory, NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            _worldFactory = worldFactory;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;

            IsCreatedBuilding = false;
            _isHided = true;

            _nextBuildingForPlacingCreator.DataChanged += OnNextBuildingForPlacingDataChanged;
        }

        public TileRepresentation MarkedTile { get; private set; }
        public bool IsCreatedBuilding { get; private set; }
        public BuildingType BuildingType => _building.Type;

        private void OnDestroy() =>
            _nextBuildingForPlacingCreator.DataChanged -= OnNextBuildingForPlacingDataChanged;

        public void Mark(TileRepresentation tile)
        {
            transform.position = tile.BuildingPoint.position;
            MarkedTile = tile;
        }

        public void Show()
        {
            if (_building != null)
            {
                _building.gameObject.SetActive(true);
                _building.Blink();
            }

            _isHided = false;
        }

        public void Hide()
        {
            if (_building != null)
            {
                _building.gameObject.SetActive(false);
                _building.StopBlinking();
            }

            _isHided = true;
        }

        public void Replace(Vector3 targetPosition) =>
            transform.position = targetPosition;

        private async UniTask TryUpdate(BuildingType targetBuildingType)
        {
            if (targetBuildingType == BuildingType.Undefined)
                Debug.LogError("Building type can not be undefined");

            if (IsCreatedBuilding)
                Debug.LogError("The building is not yet complete");

            if (_building == null || _building.Type != targetBuildingType)
            {
                _building?.Destroy();

                IsCreatedBuilding = true;

                _building = await _worldFactory.CreateBuilding(targetBuildingType, transform.position, transform);

                if (_building.transform.position != transform.position)
                {
                    _building.transform.position = transform.position;

                    Debug.LogWarning("The building's position has shifted");
                }

                //_building.Blink();

                if (_isHided)
                    _building.gameObject.SetActive(false);

                IsCreatedBuilding = false;
            }
        }

        private async void OnNextBuildingForPlacingDataChanged(BuildingsForPlacingData data) =>
            await TryUpdate(data.CurrentBuildingType);

        public class Factory : PlaceholderFactory<string, UniTask<BuildingMarker>>
        {
        }
    }
}
