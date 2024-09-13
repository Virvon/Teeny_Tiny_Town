using Assets.Sources.Infrastructure.GameplayFactory;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

namespace Assets.Sources.Gameplay.WorldGenerator
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private Tile.Tile _tile;
        [SerializeField] private uint _length;
        [SerializeField] private uint _width;
        [SerializeField] private float _cellSize;

        private IGameplayFactory _gameplayFactory;
        private List<Tile.Tile> _tiles;

        public IReadOnlyList<Tile.Tile> Tiles => _tiles;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory)
        {
            _gameplayFactory = gameplayFactory;

            _tiles = new();
        }

        public async UniTask Generate()
        {
            await Fill();
            InitTiles();
        }

        private async UniTask Fill()
        {
            List<UniTask> tasks = new();

            for (int x = 0; x < _length; x++)
            {
                for (int z = 0; z < _width; z++)
                {
                    tasks.Add(Create(new Vector2Int(x, z)));
                }
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask Create(Vector2Int gridPosition)
        {
            Vector3 worldPosition = GridToWorldPosition(gridPosition);
            Tile.Tile tile = await _gameplayFactory.CreateTile(worldPosition, transform);
            tile.TileMerge.Init(gridPosition);
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

        private void InitTiles()
        {
            foreach(Tile.Tile tile in _tiles)
            {
                List<Tile.Tile> adjacentTiles = new();

                Vector2Int tileGridPosition = tile.TileMerge.GridPosition;

                for (int x = tileGridPosition.x - 1; x <= tileGridPosition.x + 1; x++)
                {
                    if (x < 0 || x >= _length || x == tileGridPosition.x )
                        continue;

                    adjacentTiles.Add(_tiles.Where(adjacentTile => adjacentTile.TileMerge.GridPosition == new Vector2Int(x, tileGridPosition.y)).First());
                }

                for (int y = tileGridPosition.y - 1; y <= tileGridPosition.y + 1; y++)
                {
                    if (y < 0 || y >= _width || y == tileGridPosition.y)
                        continue;

                    adjacentTiles.Add(_tiles.Where(adjacentTile => adjacentTile.TileMerge.GridPosition == new Vector2Int(tileGridPosition.x, y)).First());
                }

                tile.TileMerge.Init(adjacentTiles);
            }
        }

        public class Factory : PlaceholderFactory<string, UniTask<WorldGenerator>>
        {
        }
    }
}
