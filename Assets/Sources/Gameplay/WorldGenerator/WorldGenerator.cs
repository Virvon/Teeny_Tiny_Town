using Assets.Sources.Infrastructure.GameplayFactory;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.WorldGenerator
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private Tile.Tile _tile;
        [SerializeField] private float _cellSize;

        private IGameplayFactory _gameplayFactory;
        private World.World _world;

        private List<Tile.Tile> _tiles;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory, World.World world)
        {
            _gameplayFactory = gameplayFactory;
            _world = world;

            _tiles = new();
        }

        public async UniTask Generate()
        {
            await Fill();
        }

        public Tile.Tile GetTile(Vector2Int gridPosition)
        {
            return _tiles.First(tile => tile.GridPosition == gridPosition);
        }

        private async UniTask Fill()
        {
            List<UniTask> tasks = new();

            foreach(World.TileModel tileModel in _world.Tiles)
                tasks.Add(Create(tileModel.GridPosition));

            await UniTask.WhenAll(tasks);
        }

        private async UniTask Create(Vector2Int gridPosition)
        {
            Vector3 worldPosition = GridToWorldPosition(gridPosition);
            Tile.Tile tile = await _gameplayFactory.CreateTile(worldPosition, transform);
            tile.Init(gridPosition);
            _tiles.Add(tile);
        }

        

        private Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector3(
                gridPosition.x * _cellSize,
                transform.position.y,
                gridPosition.y * _cellSize);
        }

        private Vector3Int WorldToGridPosition(Vector3 worldPosition)
        {
            return new Vector3Int(
                (int)(worldPosition.x / _cellSize),
                (int)(worldPosition.y / _cellSize),
                (int)(worldPosition.z / _cellSize));
        }

        public class Factory : PlaceholderFactory<string, UniTask<WorldGenerator>>
        {
        }
    }
}
