using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using TileRepresentation = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.TileRepresentation;
using Building = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Building;
using Ground = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds.Ground;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Gameplay.World;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly DiContainer _container;
        private readonly TileRepresentation.Factory _tileFactory;
        private readonly Building.Factory _buildingFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly SelectFrame.Factory _selectFrameFactory;
        private readonly Ground.Factory _groundFactory;
        private readonly BuildingMarker.Factory _buildingMarkerFactory;
        private readonly WorldsList.Factory _worldsListFactory;
        private readonly World.Factory _worldFactory;

        public GameplayFactory(
            DiContainer container,
            TileRepresentation.Factory tileFactory,
            Building.Factory buildingFactory,
            IStaticDataService staticDataService,
            SelectFrame.Factory selectFrameFactory,
            Ground.Factory groundFactory,
            BuildingMarker.Factory buildingMarkerFactory,
            WorldsList.Factory worldsListFactory,
            World.Factory worldFactory)
        {
            _container = container;
            _tileFactory = tileFactory;
            _buildingFactory = buildingFactory;
            _staticDataService = staticDataService;
            _selectFrameFactory = selectFrameFactory;
            _groundFactory = groundFactory;
            _buildingMarkerFactory = buildingMarkerFactory;
            _worldsListFactory = worldsListFactory;
            _worldFactory = worldFactory;
        }

        public async UniTask CreateWorld(Vector3 position, Transform parent)
        {
            await _worldFactory.Create(GameplayFactoryAssets.World, position, parent);
        }

        public async UniTask<WorldsList> CreateWorldsList()
        {
            WorldsList worldsList = await _worldsListFactory.Create(GameplayFactoryAssets.WorldsList);

            return worldsList;
        }

        public async UniTask CreateBuildingMarker()
        {
            BuildingMarker marker = await _buildingMarkerFactory.Create(GameplayFactoryAssets.BuildingMarker);

            _container.Bind<BuildingMarker>().FromInstance(marker).AsSingle();
        }

        public async UniTask<Ground> CreateGround(GroundType groundType, RoadType roadType, Vector3 position, GroundRotation rotation, Transform parent) =>
            await _groundFactory.Create(_staticDataService.GetRoad(groundType, roadType).AssetReference, position, (int)rotation, parent);

        

        public async UniTask CreateSelectFrame()
        {
            SelectFrame selectFrame = await _selectFrameFactory.Create(GameplayFactoryAssets.SelectFrame);

            _container.Bind<SelectFrame>().FromInstance(selectFrame).AsSingle();
        }

        

        public async UniTask<TileRepresentation> CreateTile(Vector3 position, Transform parent)
        {
            TileRepresentation tile = await _tileFactory.Create(GameplayFactoryAssets.Tile, position, parent);

            return tile;
        }

        public async UniTask<Building> CreateBuilding(BuildingType type, Vector3 position, Transform parent)
        {
            BuildingConfig buildingConfig = _staticDataService.GetBuilding<BuildingConfig>(type);

            Building building = await _buildingFactory.Create(buildingConfig.AssetReference, position, parent);

            building.Init(type);

            return building;
        }
    }
}
