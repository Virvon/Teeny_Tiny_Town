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

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory)
        {
            _gameplayFactory = gameplayFactory;

            _buildingPositionHandler.BuildingCreated += OnBuildingCreated;
        }

        public async UniTask Create()
        {
            Tile.Tile tile = GetRandomEmptyTile();

            Building building = await _gameplayFactory.CreateBuilding(tile.GetComponent<GroundCreator>().Ground.transform.position, tile.transform);

            building.Ground = tile.GetComponent<GroundCreator>().Ground;

            _buildingPositionHandler.Set(building);
        }

        private Tile.Tile GetRandomEmptyTile()
        {
            return _worldGenerator.Tiles[Random.Range(0, _worldGenerator.Tiles.Count - 1)];
        }

        private async void OnBuildingCreated()
        {
            await Create();
        }
    }
}
