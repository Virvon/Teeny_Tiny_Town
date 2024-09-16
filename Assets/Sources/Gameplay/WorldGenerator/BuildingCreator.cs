using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Infrastructure.GameplayFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.WorldGenerator
{
    public class BuildingCreator : MonoBehaviour
    {
        [SerializeField] private Building _buildingPrefab;
        [SerializeField] private WorldGenerator _worldGenerator;
        [SerializeField] private BuildingPositionHandler _buildingPositionHandler;

        private IGameplayFactory _gameplayFactory;
        private World.World _world;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory, World.World world)
        {
            _gameplayFactory = gameplayFactory;
            _world = world;

            _buildingPositionHandler.BuildingCreated += OnBuildingCreated;
            _world.TileCleaned += OnTileCleaned;
        }

        private void OnDestroy()
        {
            _buildingPositionHandler.BuildingCreated -= OnBuildingCreated;
            _world.TileCleaned -= OnTileCleaned;
        }

        public async UniTask Create()
        {
            Tile.Tile tile = GetRandomEmptyTile();

            Building building = await _gameplayFactory.CreateBuilding(tile.GetComponent<GroundCreator>().Ground.transform.position, tile.transform);

            _buildingPositionHandler.Set(building, tile);
        }

        private Tile.Tile GetRandomEmptyTile()
        {
            return _worldGenerator.GetTile(_world.Tiles[Random.Range(0, _world.Tiles.Count - 1)].GridPosition);
        }

        private async void OnBuildingCreated()
        {
            await Create();
        }

        private void OnTileCleaned(Vector2Int gridPosition)
        {
            _worldGenerator.GetTile(gridPosition).Clean();
        }
    }
}
