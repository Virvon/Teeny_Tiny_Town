using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Gameplay.WorldGenerator;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Tile = Assets.Sources.Gameplay.Tile.Tile;

namespace Assets.Sources.Infrastructure.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly DiContainer _container;
        private readonly WorldGenerator.Factory _worldGeneratorFactory;
        private readonly Tile.Factory _tileFactory;
        private readonly Building.Factory _buildingFactory;

        public GameplayFactory(DiContainer container, WorldGenerator.Factory worldGeneratorFactory, Tile.Factory tileFactory, Building.Factory buildingFactory)
        {
            _container = container;
            _worldGeneratorFactory = worldGeneratorFactory;
            _tileFactory = tileFactory;
            _buildingFactory = buildingFactory;
        }

        public async UniTask CreateWorldGenerator()
        {
            WorldGenerator worldGenerator = await _worldGeneratorFactory.Create(GameplayFactoryAssets.WorldGenerator);

            _container.Bind<WorldGenerator>().FromInstance(worldGenerator).AsSingle();
            _container.Bind<BuildingCreator>().FromInstance(worldGenerator.GetComponent<BuildingCreator>()).AsSingle();
        }

        public async UniTask<Tile> CreateTile(Vector3 position, Transform parent)
        {
            Tile tile = await _tileFactory.Create(GameplayFactoryAssets.Tile, position, parent);

            return tile;
        }

        public async UniTask<Building> CreateBuilding(Vector3 position, Transform parent)
        {
            Building building = await _buildingFactory.Create(GameplayFactoryAssets.Building, position, parent);

            return building;
        }
    }
}
