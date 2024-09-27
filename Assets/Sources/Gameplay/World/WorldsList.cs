using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldsList : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private IGameplayFactory _gameplayFactory;

        private int _currentWorldNumber;
        private float _distanceBetweenWorlds;
        private List<World> _worlds;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IGameplayFactory gameplayFactory)
        {
            _staticDataService = staticDataService;
            _gameplayFactory = gameplayFactory;

            _currentWorldNumber = 0;
            _distanceBetweenWorlds = _staticDataService.WorldsConfig.DistanceBetweenWorlds;
            _worlds = new();
        }

        public async UniTask Create()
        {
            Vector3 position = Vector3.zero;

            foreach(WorldConfig worldConfig in _staticDataService.WorldsConfig.Configs)
            {
                World world = await _gameplayFactory.CreateWorld(position, transform);

                position += Vector3.right * _distanceBetweenWorlds;
                _worlds.Add(world);
            }
        }

        public void ShowNextWorld()
        {
            _currentWorldNumber++;
            transform.position = Vector3.right * _currentWorldNumber * _distanceBetweenWorlds;
        }

        public void ShowPreviousWorld()
        {
            _currentWorldNumber--;
            transform.position = Vector3.right * _currentWorldNumber * _distanceBetweenWorlds;
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
