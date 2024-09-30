using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldsList : MonoBehaviour
    {
        private IGameplayFactory _gameplayFactory;
        private IPersistentProgressService _persistentProgressService;
        private int _currentWorldNumber;
        private float _distanceBetweenWorlds;
        private List<World> _worlds;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IGameplayFactory gameplayFactory, IPersistentProgressService persistentProgressService)
        {
            _gameplayFactory = gameplayFactory;
            _persistentProgressService = persistentProgressService;
            _currentWorldNumber = 0;
            _distanceBetweenWorlds = staticDataService.WorldsConfig.DistanceBetweenWorlds;
            _worlds = new();
        }

        public async UniTask Create()
        {
            Vector3 position = Vector3.zero;

            foreach(WorldData worldData in _persistentProgressService.Progress.WorldDatas)
            {
                World world = await _gameplayFactory.CreateWorld(position, transform);

                world.Init(worldData);

                position += Vector3.right * _distanceBetweenWorlds;
                _worlds.Add(world);
            }
        }

        public void ShowNextWorld()
        {
            _currentWorldNumber++;
            transform.position = new Vector3(-_currentWorldNumber * _distanceBetweenWorlds, 0, 0);
        }

        public void ShowPreviousWorld()
        {
            _currentWorldNumber--;
            transform.position = new Vector3(-_currentWorldNumber * _distanceBetweenWorlds, 0, 0);
        }

        public void StartCurrentWorld()
        {
            _worlds[_currentWorldNumber].Choose();
        }

        public class Factory : PlaceholderFactory<string, UniTask<WorldsList>>
        {
        }
    }
}
