using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Collection
{
    public class CollectionItemCreator : MonoBehaviour
    {
        [SerializeField] private float _distanceBetweenItems;

        private IPersistentProgressService _persistentProgressService;
        private IWorldFactory _worldFactory;
        private IStaticDataService _staticDataService;
        private AnimationsConfig _animationsConfig;

        private TileRepresentation _currentTile;
        private Vector3 _nextItemPosition;
        private Vector3 _previousItemPosition;
        private bool _canChangeItems;

        [Inject]
        private void Construct(
            IPersistentProgressService persistentProgressService,
            IWorldFactory worldFactory,
            IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _worldFactory = worldFactory;
            _staticDataService = staticDataService;
            _animationsConfig = _staticDataService.AnimationsConfig;

            CollectionItemIndex = 0;
            _nextItemPosition = new Vector3(transform.position.x + _distanceBetweenItems, transform.position.y, transform.position.z + _distanceBetweenItems);
            _previousItemPosition = new Vector3(transform.position.x - _distanceBetweenItems, transform.position.y, transform.position.z - _distanceBetweenItems);
            _canChangeItems = true;
        }

        public event Action<uint> ItemChanged;

        public int CollectionItemIndex { get; private set; }

        private async void Start() =>
            _currentTile = await CreateItem(transform.position);

        public async UniTask ShowNextBuilding()
        {
            if (_canChangeItems == false)
                return;

            _canChangeItems = false;
            CollectionItemIndex++;

            if (CollectionItemIndex >= _persistentProgressService.Progress.BuildingDatas.Length)
                CollectionItemIndex = 0;

            await ChangeTiles(_previousItemPosition, _nextItemPosition, callback: () => _canChangeItems = true);
        }

        public async UniTask ShowPreviousBuilding()
        {
            if (_canChangeItems == false)
                return;

            _canChangeItems = false;
            CollectionItemIndex--;

            if (CollectionItemIndex < 0)
                CollectionItemIndex = _persistentProgressService.Progress.BuildingDatas.Length - 1;

            await ChangeTiles(_nextItemPosition, _previousItemPosition, callback: () => _canChangeItems = true);
        }

        private async UniTask ChangeTiles(Vector3 destroyedTileTargetPosition, Vector3 currentTileStartPosition, TweenCallback callback)
        {
            ItemChanged?.Invoke(_persistentProgressService.Progress.BuildingDatas[CollectionItemIndex].Count);

            TileRepresentation destroyedTile = _currentTile;
            _currentTile = await CreateItem(currentTileStartPosition);

            destroyedTile.transform.DOMove(destroyedTileTargetPosition, _animationsConfig.CollectionItemMoveDuration).onComplete += destroyedTile.Destroy;
            _currentTile.transform.DOMove(transform.position, _animationsConfig.CollectionItemMoveDuration).onComplete = callback;
        }

        private async UniTask<TileRepresentation> CreateItem(Vector3 position)
        {   
            BuildingType buildingType = _persistentProgressService.Progress.BuildingDatas[CollectionItemIndex].Type;

            TileRepresentation tile = await _worldFactory.CreateTileRepresentation(position, transform);

            if (buildingType == BuildingType.Lighthouse)
                await tile.GroundCreator.Create(TileType.WaterSurface);
            else
                await tile.GroundCreator.Create(_staticDataService.GetGroundType(buildingType), RoadType.NonEmpty, GroundRotation.Degrees0, false);

            await tile.TryChangeBuilding<BuildingRepresentation>(buildingType, true);

            return tile;
        }

        public class Factory : PlaceholderFactory<string, UniTask<CollectionItemCreator>>
        {
        }
    }
}
