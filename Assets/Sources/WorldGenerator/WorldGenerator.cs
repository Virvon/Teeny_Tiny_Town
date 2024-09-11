using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.WorldGenerator
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private Tile _tile;
        [SerializeField] private uint _length;
        [SerializeField] private uint _width;
        [SerializeField] private float _cellSize;
        [SerializeField] private BuildingPositionHandler buildingPositionHandler;

        private void Awake()
        {
            Fill();
        }

        private void Fill()
        {
            for(int x = 0; x < _length; x++)
            {
                for(int z = 0; z < _width; z++)
                {
                    Create(new Vector3Int(x, (int)transform.position.y, z));
                }
            }
        }

        private void Create(Vector3Int gridPosition)
        {
            Vector3 worldPosition = GridToWorldPosition(gridPosition);
            Tile tile = Instantiate(_tile, worldPosition, Quaternion.identity, transform);
            buildingPositionHandler.Add(tile);
        }

        private Vector3 GridToWorldPosition(Vector3Int gridPosition)
        {
            return new Vector3(
                gridPosition.x * _cellSize,
                gridPosition.y * _cellSize,
                gridPosition.z * _cellSize);
        }

        private Vector3Int WorldToGridPosition(Vector3 worldPosition)
        {
            return new Vector3Int(
                (int)(worldPosition.x / _cellSize),
                (int)(worldPosition.y / _cellSize),
                (int)(worldPosition.z / _cellSize));
        }
    }
}
