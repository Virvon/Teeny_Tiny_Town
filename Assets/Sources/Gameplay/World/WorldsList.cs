using System;
using System.Linq;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
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
        private IStaticDataService _staticDataService;

        private World _currentWorld;
        private bool _isWorldChanged;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IGameplayFactory gameplayFactory, IPersistentProgressService persistentProgressService)
        {
            _staticDataService = staticDataService;
            _currentWorldPosition = _staticDataService.WorldsConfig.CurrentWorldPosition;

            _gameplayFactory = gameplayFactory;
            _persistentProgressService = persistentProgressService;

            float distanceBetweenWorlds = _staticDataService.WorldsConfig.DistanceBetweenWorlds;
            _previousWorldPosition = new Vector3(_currentWorldPosition.x - distanceBetweenWorlds, _currentWorldPosition.y, _currentWorldPosition.z);
            _nextWorldPosition = new Vector3(_currentWorldPosition.x + distanceBetweenWorlds, _currentWorldPosition.y, _currentWorldPosition.z);

            _isWorldChanged = false;
        }

        public event Action<string> CurrentWorldChanged;

        public string CurrentWorldDataId { get; private set; }

        public void CleanCurrentWorld() =>
            _currentWorld.Clean();

        public async UniTask Initialize()
        {
            if (_persistentProgressService.Progress.IsEducationCompleted)
                _currentWorld = await _gameplayFactory.CreateWorld(_persistentProgressService.Progress.LastPlayedWorldDataId, _currentWorldPosition, transform);
            else
                _currentWorld = await _gameplayFactory.CreateEducationWorld(_currentWorldPosition, transform);

            CurrentWorldDataId = _persistentProgressService.Progress.LastPlayedWorldDataId;
        }

        public async UniTask ShowNextWorld()
        {
            WorldConfig config = _staticDataService.GetWorld<WorldConfig>(CurrentWorldDataId);
            await ChangeWorld(config.NextWorldId, _nextWorldPosition, _previousWorldPosition);
        }

        public async UniTask ShowPreviousWorld()
        {
            WorldConfig config = _staticDataService.GetWorld<WorldConfig>(CurrentWorldDataId);
            await ChangeWorld(config.PreviousWorldId, _previousWorldPosition, _nextWorldPosition);
        }

        public async UniTask StartLastPlayedWorld()
        {
            if (_isWorldChanged)
                await UniTask.WaitWhile(() => _isWorldChanged);

            if (CurrentWorldDataId != _persistentProgressService.Progress.LastPlayedWorldDataId)
            {
                await ChangeWorld(_persistentProgressService.Progress.LastPlayedWorldDataId, _nextWorldPosition, _previousWorldPosition);
                await UniTask.WaitWhile(() => _isWorldChanged);
            }

            _currentWorld.Enter();
        }

        public async UniTask StartCurrentWorld()
        {
            if (_isWorldChanged)
                await UniTask.WaitWhile(() => _isWorldChanged);

            _currentWorld.Enter();
        }

        public async UniTask ChangeToEducationWorld(Action callback)
        {
            _isWorldChanged = true;

            WorldConfig worldConfig = _staticDataService.GetWorld<WorldConfig>(_staticDataService.WorldsConfig.EducationWorldId);
            CurrentWorldDataId = _staticDataService.WorldsConfig.EducationWorldId;
            _persistentProgressService.Progress.LastPlayedWorldDataId = CurrentWorldDataId;

            _persistentProgressService.Progress.GetWorldData(CurrentWorldDataId).Update(worldConfig.TilesDatas, worldConfig.NextBuildingTypeForCreation, worldConfig.StartingAvailableBuildingTypes.ToList());

            World world = await _gameplayFactory.CreateEducationWorld(_nextWorldPosition, transform);

            ReplaceWorlds(world, _previousWorldPosition, _currentWorldPosition, () =>
            {
                _currentWorld = world;
                _isWorldChanged = false;
                CurrentWorldChanged?.Invoke(CurrentWorldDataId);
                callback?.Invoke();
            });
        }

        private async UniTask ChangeWorld(string targetWorldDataId, Vector3 newWorldStartPosition, Vector3 currentWorldTargetPosition)
        {
            if (_isWorldChanged)
                return;

            _isWorldChanged = true;

            CurrentWorldDataId = targetWorldDataId;
            CurrentWorldChanged?.Invoke(CurrentWorldDataId);

            World world = await _gameplayFactory.CreateWorld(targetWorldDataId, newWorldStartPosition, transform);

            ReplaceWorlds(world, currentWorldTargetPosition, _currentWorldPosition, () =>
            {
                _currentWorld = world;
                _isWorldChanged = false;
            });
        }

        private void ReplaceWorlds(World changedWorld, Vector3 currentWorldTargetPosition, Vector3 changedWorldTargetPosition, TweenCallback callback)
        {
            World currentWorld = _currentWorld;

            currentWorld.MoveTo(currentWorldTargetPosition, callback: () => Destroy(currentWorld.gameObject));
            changedWorld.MoveTo(changedWorldTargetPosition, callback);
        }

        public class Factory : PlaceholderFactory<string, UniTask<WorldsList>>
        {
        }
    }
}
