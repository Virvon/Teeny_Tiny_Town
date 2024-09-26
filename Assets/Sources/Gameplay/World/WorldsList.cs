using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldsList : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private IGameplayFactory _gameplayFactory;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IGameplayFactory gameplayFactory)
        {
            _staticDataService = staticDataService;
            _gameplayFactory = gameplayFactory;
        }

        public async UniTask Create()
        {
            Vector3 position = Vector3.zero;

            foreach(WorldConfig worldConfig in _staticDataService.WorldsConfig.Configs)
            {
                await _gameplayFactory.CreateWorld(position, transform);

                position += Vector3.right * _staticDataService.WorldsConfig.DistanceBetweenWorlds;
            }
        }

        public class Factory : PlaceholderFactory<string, UniTask<WorldsList>>
        {
        }
    }
}
