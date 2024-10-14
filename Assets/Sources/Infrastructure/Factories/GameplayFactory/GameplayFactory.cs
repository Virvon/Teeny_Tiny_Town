using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Camera;
using Assets.Sources.Services.StaticDataService.Configs.World;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly DiContainer _container;
        private readonly WorldsList.Factory _worldsListFactory;
        private readonly World.Factory _worldFactory;
        private readonly GameplayCamera.Factory _gameplayCameraFactory;
        private readonly IStaticDataService _staticDataService;

        public GameplayFactory(
            DiContainer container,
            WorldsList.Factory worldsListFactory,
            World.Factory worldFactory,
            GameplayCamera.Factory gameplayCameraFactory,
            IStaticDataService staticDataService)
        {
            _container = container;
            _worldsListFactory = worldsListFactory;
            _worldFactory = worldFactory;
            _gameplayCameraFactory = gameplayCameraFactory;
            _staticDataService = staticDataService;
        }

        public async UniTask<GameplayCamera> CreateCamera(GameplayCameraType type) =>
            await _gameplayCameraFactory.Create(_staticDataService.GetGameplayCamera(type).AssetReference);

        public async UniTask<World> CreateWorld(string id, Vector3 position, Transform parent)
        {
            return await _worldFactory.Create(_staticDataService.GetWorld<WorldConfig>(id).AssetReference, position, parent);
        }

        public async UniTask<WorldsList> CreateWorldsList()
        {
            WorldsList worldsList = await _worldsListFactory.Create(GameplayFactoryAssets.WorldsList);

            _container.BindInstance(worldsList).AsSingle();

            return worldsList;
        }
    }
}
