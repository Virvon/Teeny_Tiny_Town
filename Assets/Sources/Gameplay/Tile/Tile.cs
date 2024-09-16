using Assets.Sources.Gameplay.WorldGenerator;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Tile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GroundCreator _groundCreator;
        [SerializeField] private BuildingCreator _buildingCreator;

        private Building _building;

        public Vector2Int GridPosition { get; private set; }

        public void Init(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }

        public void Select(SelectFrame selectFrame, Vector3 selectFramePositionOffset)
        {
            selectFrame.transform.position = _groundCreator.Ground.BuildingPoint.position + selectFramePositionOffset;
        }

        public void PutBuilding(Building building)
        {
            building.transform.position = _groundCreator.Ground.BuildingPoint.position;
        }

        public void SetBuilding(Building building)
        {
            _building = building;
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

        private void DestroyBuilding()
        {
            Destroy(_building.gameObject);
            _building = null;
        }

        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<Tile>>
        {
        }

        
    }
}
