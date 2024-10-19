using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class TileRepresentation : MonoBehaviour
    {
        [SerializeField] private GroundCreator _groundCreator;
        [SerializeField] private BuildingCreator _buildingCreator;

        private BuildingRepresentation _building;

        public Vector2Int GridPosition { get; private set; }
        public bool IsEmpty => _building == null;
        public BuildingType BuildingType => IsEmpty ? BuildingType.Undefined : _building.Type;
        public Transform BuildingPoint => _groundCreator.Ground.BuildingPoint;
        public GroundCreator GroundCreator => _groundCreator;

        public void Init(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }

        public BuildingRepresentation TakeBuilding()
        {
            BuildingRepresentation building = _building;

            _building = null;

            return building;
        }

        public void ShakeBuilding() =>
            _building.Shake();

        public void StopBuildingShaking() =>
            _building.StopShaking();

        public void RaiseBuilding(Vector3 offset) =>
            _building.transform.position += offset;

        public void LowerBuilding() =>
            PlaceBuildingToBuildingPoint(_building);

        public void PlaceBuilding(BuildingRepresentation building)
        {
            if (IsEmpty == false)
                Debug.LogError("can not place building to non empty tile");

            _building = building;

            PlaceBuildingToBuildingPoint(building);
        }

        public void DestroyBuilding()
        {
            _building.Destroy();
            _building = null;
        }

        public void DestroyBuilding(Vector3 destroyPosition)
        {
            _building.AnimateDestroy(destroyPosition);
            _building = null;
        }

        public async UniTask AnimateDestroyBuilding()
        {
            await _building.AnimateDestroy();
            _building = null;
        }

        public async UniTask<TBuilding> TryChangeBuilding<TBuilding>(BuildingType targetBuildingType, bool waitForCompletion)
            where TBuilding : BuildingRepresentation
        {
            if (BuildingType != targetBuildingType)
            {
                if (IsEmpty == false)
                    DestroyBuilding();

                if (targetBuildingType != BuildingType.Undefined)
                {
                    _building = await _buildingCreator.Create(targetBuildingType);  
                    await _building.AnimatePut(waitForCompletion);

                    return _building as TBuilding;
                }
            }

            return null;
        }

        public void Destroy() =>
            Destroy(gameObject);

        private void PlaceBuildingToBuildingPoint(BuildingRepresentation building) =>
            building.transform.position = _groundCreator.Ground.BuildingPoint.position;

        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<TileRepresentation>>
        {
        }
    }
}
