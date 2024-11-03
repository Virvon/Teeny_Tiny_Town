using Assets.Sources.Data.World;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Linq;
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

        public async UniTask CreateCurrentWorld()
        {
            if (_persistentProgressService.Progress.IsEducationCompleted)
                _currentWorld = await _gameplayFactory.CreateWorld(_persistentProgressService.Progress.CurrentWorldData.Id, _currentWorldPosition, transform);
            else
                _currentWorld = await _gameplayFactory.CreateEducationWorld(_currentWorldPosition, transform);

        }

        public async UniTask ShowNextWorld()
        {
            if (_isWorldChanged)
                return;

            _isWorldChanged = true;

            WorldData nextWorldData = _persistentProgressService.Progress.GetNextWorldData();
            World world = await _gameplayFactory.CreateWorld(nextWorldData.Id, _nextWorldPosition, transform);

            ReplaceWorlds(world, _previousWorldPosition, _currentWorldPosition, () =>
            {
                _currentWorld = world;
                _isWorldChanged = false;                
            });
        }

        public async UniTask ShowPreviousWorld()
        {
            if (_isWorldChanged)
                return;

            _isWorldChanged = false;

            WorldData previousWorldData = _persistentProgressService.Progress.GetPreviousWorldData();
            World world = await _gameplayFactory.CreateWorld(previousWorldData.Id, _previousWorldPosition, transform);

            ReplaceWorlds(world, _nextWorldPosition, _currentWorldPosition, () =>
            {
                _currentWorld = world;
                _isWorldChanged = false;
            });
        }

        public void StartCurrentWorld()
        {
            if(_isWorldChanged == false)
                _currentWorld.EnterBootstrapState();
        }

        public async UniTask ChangeToEducationWorld(Action callback)
        {
            _isWorldChanged = true;

            WorldConfig worldConfig = _staticDataService.GetWorld<WorldConfig>(_staticDataService.WorldsConfig.EducationWorldId);

            WorldData nextWorldData = _persistentProgressService.Progress.ChangeWorldData(_staticDataService.WorldsConfig.EducationWorldId);

            nextWorldData.Update(worldConfig.TilesDatas, worldConfig.NextBuildingTypeForCreation, worldConfig.StartingAvailableBuildingTypes.ToList());

            World world = await _gameplayFactory.CreateEducationWorld(_nextWorldPosition, transform);

            ReplaceWorlds(world, _previousWorldPosition, _currentWorldPosition, () =>
            {
                _currentWorld = world;
                _isWorldChanged = false;
                callback?.Invoke();
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
