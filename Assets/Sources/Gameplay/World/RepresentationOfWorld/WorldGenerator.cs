using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private TileRepresentation _tile;
        [SerializeField] private float _cellSize;

        private IWorldFactory _worldFactory;
        private WorldChanger _world;

        private List<TileRepresentation> _tiles;

        [Inject]
        private void Construct(IWorldFactory worldFactory, WorldChanger world)
        {
            _worldFactory = worldFactory;
            _world = world;

            _tiles = new();
        }

        public async UniTask Generate() =>
            await Fill();

        public TileRepresentation GetTile(Vector2Int gridPosition) =>
            _tiles.First(tile => tile.GridPosition == gridPosition);

        private async UniTask Fill()
        {
            List<UniTask> tasks = new();

            foreach (Tile tileModel in _world.Tiles)
                tasks.Add(Create(tileModel.GridPosition));

            await UniTask.WhenAll(tasks);
        }

        private async UniTask Create(Vector2Int gridPosition)
        {
            Vector3 worldPosition = GridToWorldPosition(gridPosition) + transform.position;
            TileRepresentation tileRepresentation = await _worldFactory.CreateTileRepresentation(worldPosition, transform);
            Tile tile = _world.GetTile(gridPosition);

            tileRepresentation.Init(gridPosition);
            await tileRepresentation.Change(tile.BuildingType, tile.Ground.Type, tile.Ground.RoadType, tile.Ground.Rotation);

            _tiles.Add(tileRepresentation);
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
