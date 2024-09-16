using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Infrastructure.GameplayFactory;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
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
            _world.TileBuildingUpdated += OnTileBuildingUpdated;
        }

        private void OnDestroy()
        {
            _buildingPositionHandler.BuildingCreated -= OnBuildingCreated;
            _world.TileCleaned -= OnTileCleaned;
            _world.TileBuildingUpdated -= OnTileBuildingUpdated;
        }

        public async UniTask Create()
        {
            Tile.Tile tile = GetRandomEmptyTile();
            Building building = await CreateBuilding(tile, BuildingType.Bush);

            _buildingPositionHandler.Set(building, tile);
        }

        private async Task<Building> CreateBuilding(Tile.Tile tile, BuildingType type)
        {
            return await _gameplayFactory.CreateBuilding(type, tile.GetComponent<GroundCreator>().Ground.transform.position, tile.transform);
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

        private async void OnTileBuildingUpdated(Vector2Int gridPosition, BuildingType buildingType)
        {
            Tile.Tile tile = _worldGenerator.GetTile(gridPosition);
            Building building = await CreateBuilding(tile, buildingType);

            tile.PutBuilding(building);
            tile.SetBuilding(building);

            _world.Update(tile.GridPosition, buildingType);
        }
    }
}
