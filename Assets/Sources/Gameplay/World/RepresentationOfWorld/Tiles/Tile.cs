using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GroundCreator _groundCreator;
        [SerializeField] private BuildingCreator _buildingCreator;

        private Building _building;

        public Vector2Int GridPosition { get; private set; }
        public bool IsEmpty => _building == null;
        public BuildingType BuildingType => IsEmpty ? BuildingType.Undefined : _building.Type;
        public Transform BuildingPoint => _groundCreator.Ground.BuildingPoint;

        public async UniTask Init(Vector2Int gridPosition, GroundType groundType, GroundRotation groundRotation)
        {
            GridPosition = gridPosition;
            await _groundCreator.Create(groundType, groundRotation);
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

        public void Replace(Building building)
        {
            if (IsEmpty)
                PlaceBuildingToBuildingPoint(building);
        }


        public void PlaceBuilding(Building building)
        {
            if (IsEmpty == false)
                Debug.LogError("can not place building to non empty tile");

            _building = building;

            PlaceBuildingToBuildingPoint(building);

        }

        public async UniTask<Building> CreateBuilding(BuildingType type)
        {
            if (_building != null)
                DestroyBuilding();

            if (type != BuildingType.Undefined)
            {
                _building = await _buildingCreator.Create(type);
                return _building;
            }

            return null;
        }

        public void DestroyBuilding()
        {
            Destroy(_building.gameObject);
            _building = null;
        }

        private void PlaceBuildingToBuildingPoint(Building building) =>
            building.transform.position = _groundCreator.Ground.BuildingPoint.position;

        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<Tile>>
        {
        }
    }
}
