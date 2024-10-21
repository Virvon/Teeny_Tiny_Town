using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class BuildingMarker : MonoBehaviour
    {
        private IWorldFactory _worldFactory;

        private BuildingRepresentation _building;

        [Inject]
        private void Construct(IWorldFactory worldFactory)
        {
            _worldFactory = worldFactory;

            IsCreatedBuilding = false;
        }

        public TileRepresentation MarkedTile { get; private set; }
        public bool IsCreatedBuilding { get; private set; }
        public BuildingType BuildingType => _building.Type;

        public void Mark(TileRepresentation tile)
        {
            transform.position = tile.BuildingPoint.position;
            MarkedTile = tile;
        }

        public void Show()
        {
            if(_building != null)
                _building.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if(_building != null)
                _building.gameObject.SetActive(false);
        }

        public void Replace(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        public async UniTask TryUpdate(BuildingType targetBuildingType)
        {
            if (targetBuildingType == BuildingType.Undefined)
                Debug.LogError("Building type can not be undefined");

            if (IsCreatedBuilding)
                Debug.LogError("The building is not yet complete");

            if(_building == null || _building.Type != targetBuildingType)
            {
                _building?.Destroy();

                IsCreatedBuilding = true;

                _building = await _worldFactory.CreateBuilding(targetBuildingType, transform.position, transform);
                _building.Blink();

                IsCreatedBuilding = false;
            }
        }

        public async UniTask<BuildingType> SwapBuilding(BuildingType newBuildingType)
        {
            BuildingType buildingType = _building.Type;
            await TryUpdate(newBuildingType);

            return buildingType;
        }

        public class Factory : PlaceholderFactory<string, UniTask<BuildingMarker>>
        {
        } 
    }
}
