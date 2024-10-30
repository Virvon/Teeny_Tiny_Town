using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldGenerator : MonoBehaviour, ITileRepresentationCreatable
    {
        [SerializeField] private TileRepresentation _tile;
        [SerializeField] private float _cellSize;

        private IWorldFactory _worldFactory;        

        private List<TileRepresentation> _tiles;

        [Inject]
        private void Construct(IWorldFactory worldFactory)
        {
            _worldFactory = worldFactory;
            
            _tiles = new();
        }

        public float CellSize => _cellSize;

        public TileRepresentation GetTile(Vector2Int gridPosition) =>
            _tiles.First(tile => tile.GridPosition == gridPosition);

        public async UniTask<TileRepresentation> Create(Vector2Int gridPosition)
        {
            Vector3 worldPosition = GridToWorldPosition(gridPosition) + transform.position;
            TileRepresentation tileRepresentation = await _worldFactory.CreateTileRepresentation(worldPosition, transform);

            tileRepresentation.Init(gridPosition);

            if (_tiles.Any(value => value.GridPosition == gridPosition))
                _tiles.Remove(GetTile(gridPosition));

            _tiles.Add(tileRepresentation);

            return tileRepresentation;
        }

        private Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector3(
                gridPosition.x * _cellSize,
                transform.position.y,
                gridPosition.y * _cellSize);
        }

        public Vector2Int WorldToGridPosition(Vector3 position)
        {
            return new Vector2Int((int)((position.x - transform.position.x) / _cellSize), (int)((position.z - transform.position.z) / _cellSize));
        }

        public class Factory : PlaceholderFactory<string, UniTask<WorldGenerator>>
        {
        }
    }
}
