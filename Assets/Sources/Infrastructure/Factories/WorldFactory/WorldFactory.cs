using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.WorldFactory
{
    public class WorldFactory : IWorldFactory
    {
        private readonly DiContainer _container;
        private readonly WorldGenerator.Factory _worldGeneratorFactory;

        public WorldFactory(DiContainer container, WorldGenerator.Factory worldGeneratorFactory)
        {
            _container = container;
            _worldGeneratorFactory = worldGeneratorFactory;
        }

        public WorldGenerator WorldGenerator { get; private set; }

        public async UniTask<WorldGenerator> CreateWorldGenerator(Transform parent)
        {
            WorldGenerator = await _worldGeneratorFactory.Create(GameplayFactoryAssets.WorldGenerator);

            _container.BindInstance(WorldGenerator).AsSingle();
            _container.BindInstance(WorldGenerator.GetComponent<BuildingCreator>()).AsSingle();

            return WorldGenerator;
        }
    }
}
