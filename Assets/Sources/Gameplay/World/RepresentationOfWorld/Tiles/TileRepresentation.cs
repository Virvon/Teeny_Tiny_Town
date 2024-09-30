using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class TileRepresentation : MonoBehaviour
    {
        [SerializeField] private GroundCreator _groundCreator;
        [SerializeField] private BuildingCreator _buildingCreator;

        private Building _building;

        public Vector2Int GridPosition { get; private set; }
        public bool IsEmpty => _building == null;
        public BuildingType BuildingType => IsEmpty ? BuildingType.Undefined : _building.Type;
        public Transform BuildingPoint => _groundCreator.Ground.BuildingPoint;

        public void Init(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }

        public Building TakeBuilding()
        {
            Building building = _building;

            _building = null;

            return building;
        }

        public void RaiseBuilding(Vector3 offset) =>
            _building.transform.position += offset;

        public void LowerBuilding() =>
            PlaceBuildingToBuildingPoint(_building);

        public void PlaceBuilding(Building building)
        {
            if (IsEmpty == false)
                Debug.LogError("can not place building to non empty tile");

            _building = building;

            PlaceBuildingToBuildingPoint(building);
        }

        public async UniTask Change(BuildingType buildingType, GroundType groundType, RoadType roadType, GroundRotation groundRotation)
        {
            await _groundCreator.Create(groundType, roadType, groundRotation);
            await TryChangeBuilding(buildingType);
        }

        public void DestroyBuilding()
        {
            Destroy(_building.gameObject);
            _building = null;
        }

        private void PlaceBuildingToBuildingPoint(Building building) =>
            building.transform.position = _groundCreator.Ground.BuildingPoint.position;

        private async UniTask TryChangeBuilding(BuildingType targetBuildingType)
        {
            if (BuildingType != targetBuildingType)
            {
                if(IsEmpty == false)
                    DestroyBuilding();

                if (targetBuildingType != BuildingType.Undefined)
                    _building = await _buildingCreator.Create(targetBuildingType);
            }
        }

        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<TileRepresentation>>
        {
        }
    }
}
