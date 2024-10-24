﻿using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using BuildingRepresentation = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings.BuildingRepresentation;
using Ground = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds.Ground;

namespace Assets.Sources.Infrastructure.Factories.WorldFactory
{
    public class WorldFactory : IWorldFactory
    {
        private readonly DiContainer _container;
        private readonly WorldGenerator.Factory _worldGeneratorFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly BuildingRepresentation.Factory _buildingFactory;
        private readonly SelectFrame.Factory _selectFrameFactory;
        private readonly TileRepresentation.Factory _tileRepresentationFactory;
        private readonly Ground.Factory _groundFactory;
        private readonly BuildingMarker.Factory _buildingMarkerFactory;
        private readonly ActionHandlerSwitcher.Factory _actionHanlderSwitcherFactory;

        public WorldFactory(
            DiContainer container,
            WorldGenerator.Factory worldGeneratorFactory,
            BuildingRepresentation.Factory buildingFactory,
            SelectFrame.Factory selectFrameFactory,
            TileRepresentation.Factory tileRepresentationFactory,
            Ground.Factory groundFactory,
            BuildingMarker.Factory buildingMarkerFactory,
            IStaticDataService staticDataService,
            ActionHandlerSwitcher.Factory actionHanlderSwitcherFactory)
        {
            _container = container;
            _worldGeneratorFactory = worldGeneratorFactory;
            _buildingFactory = buildingFactory;
            _selectFrameFactory = selectFrameFactory;
            _tileRepresentationFactory = tileRepresentationFactory;
            _groundFactory = groundFactory;
            _buildingMarkerFactory = buildingMarkerFactory;
            _staticDataService = staticDataService;
            _actionHanlderSwitcherFactory = actionHanlderSwitcherFactory;
        }

        public WorldGenerator WorldGenerator { get; private set; }

        public async UniTask CreateActionHandlerSwitcher()
        {
            await _actionHanlderSwitcherFactory.Create(WorldFactoryAssets.ActionHandlerSwitcher);
        }

        public async UniTask CreateSelectFrame()
        {
            SelectFrame selectFrame = await _selectFrameFactory.Create(WorldFactoryAssets.SelectFrame);

            _container.Bind<SelectFrame>().FromInstance(selectFrame).AsSingle();
        }

        public async UniTask<TileRepresentation> CreateTileRepresentation(Vector3 position, Transform parent)
        {
            TileRepresentation tile = await _tileRepresentationFactory.Create(WorldFactoryAssets.Tile, position, parent);

            return tile;
        }

        public async UniTask<Ground> CreateGround(TileType tileType, Vector3 position, Transform parent)
        {
            return await _groundFactory.Create(_staticDataService.GetGround(tileType).AssetReference, position, 0, parent);
        }

        public async UniTask<Ground> CreateGround(GroundType groundType, RoadType roadType, Vector3 position, GroundRotation rotation, Transform parent)
        {
            return await _groundFactory.Create(_staticDataService.GetRoad(groundType, roadType).AssetReference, position, (int)rotation, parent);
        }

        public async UniTask CreateBuildingMarker()
        {
            BuildingMarker marker = await _buildingMarkerFactory.Create(WorldFactoryAssets.BuildingMarker);

            _container.Bind<BuildingMarker>().FromInstance(marker).AsSingle();
        }

        public async UniTask<WorldGenerator> CreateWorldGenerator()
        {
            WorldGenerator = await _worldGeneratorFactory.Create(WorldFactoryAssets.WorldGenerator);

            _container.BindInstance(WorldGenerator).AsSingle();
            _container.BindInstance(WorldGenerator.GetComponent<BuildingCreator>()).AsSingle();

            return WorldGenerator;
        }

        public async UniTask<BuildingRepresentation> CreateBuilding(BuildingType type, Vector3 position, Transform parent)
        {
            BuildingConfig buildingConfig = _staticDataService.GetBuilding<BuildingConfig>(type);

            BuildingRepresentation building = await _buildingFactory.Create(buildingConfig.AssetReference, position, parent);

            building.Init(type);

            return building;
        }
    }
}
