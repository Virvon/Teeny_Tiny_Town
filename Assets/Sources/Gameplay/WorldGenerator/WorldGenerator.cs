using Assets.Sources.Infrastructure.GameplayFactory;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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

        public async UniTask Fill()
        {
            List<UniTask> tasks = new();

            for (int x = 0; x < _length; x++)
            {
                for (int z = 0; z < _width; z++)
                {
                    tasks.Add(Create(new Vector3Int(x, (int)transform.position.y, z)));
                }
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask Create(Vector3Int gridPosition)
        {
            Vector3 worldPosition = GridToWorldPosition(gridPosition);
            Tile.Tile tile = await _gameplayFactory.CreateTile(worldPosition, transform);
            _tiles.Add(tile);
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

        public class Factory : PlaceholderFactory<string, UniTask<WorldGenerator>>
        {
        }
    }
}
