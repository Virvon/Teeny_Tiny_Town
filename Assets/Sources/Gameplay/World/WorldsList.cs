using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldsList : MonoBehaviour
    {
        [SerializeField] private float _changingSpeed;

        private Vector3 _currentWorldPosition;
        private Vector3 _previousWorldPosition;
        private Vector3 _nextWorldPosition;
        private IGameplayFactory _gameplayFactory;
        private IPersistentProgressService _persistentProgressService;

        private World _currentWorld;
        private bool _canChangeWorld;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IGameplayFactory gameplayFactory, IPersistentProgressService persistentProgressService)
        {
            _currentWorldPosition = staticDataService.WorldsConfig.CurrentWorldPosition;

            _gameplayFactory = gameplayFactory;
            _persistentProgressService = persistentProgressService;

            float distanceBetweenWorlds = staticDataService.WorldsConfig.DistanceBetweenWorlds;
            _previousWorldPosition = new Vector3(_currentWorldPosition.x - distanceBetweenWorlds, _currentWorldPosition.y, _currentWorldPosition.z);
            _nextWorldPosition = new Vector3(_currentWorldPosition.x + distanceBetweenWorlds, _currentWorldPosition.y, _currentWorldPosition.z);

            _canChangeWorld = true;
        }

        public async UniTask CreateCurrentWorld() =>
            _currentWorld = await _gameplayFactory.CreateWorld(_persistentProgressService.Progress.CurrentWorldData.Id, _currentWorldPosition, transform);

        public async UniTask ShowNextWorld()
        {
            if (_canChangeWorld == false)
                return;

            _canChangeWorld = false;

            WorldData nextWorldData = _persistentProgressService.Progress.GetNextWorldData();
            World world = await _gameplayFactory.CreateWorld(nextWorldData.Id, _nextWorldPosition, transform);

            ReplaceWorlds(world, _previousWorldPosition, _currentWorldPosition, () =>
            {
                _currentWorld = world;
                _canChangeWorld = true;                
            });
        }

        public async UniTask ShowPreviousWorld()
        {
            if (_canChangeWorld == false)
                return;

            _canChangeWorld = false;

            WorldData previousWorldData = _persistentProgressService.Progress.GetPreviousWorldData();
            World world = await _gameplayFactory.CreateWorld(previousWorldData.Id, _previousWorldPosition, transform);

            ReplaceWorlds(world, _nextWorldPosition, _currentWorldPosition, () =>
            {
                _currentWorld = world;
                _canChangeWorld = true;
            });
        }

        public void StartCurrentWorld()
        {
            _currentWorld.EnterBootstrapState();
        }

        private void ReplaceWorlds(World changedWorld, Vector3 currentWorldTargetPosition, Vector3 changedWorldTargetPosition, TweenCallback callback)
        {
            Debug.Log("replace");
            World currentWorld = _currentWorld;

            currentWorld.MoveTo(currentWorldTargetPosition, callback: () => Destroy(currentWorld.gameObject));
            changedWorld.MoveTo(changedWorldTargetPosition, callback);
        }

        public class Factory : PlaceholderFactory<string, UniTask<WorldsList>>
        {
        }
    }
}
