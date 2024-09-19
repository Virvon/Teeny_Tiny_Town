using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Tile = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Tile;
using Building = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Building;
using Ground = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds.Ground;

namespace Assets.Sources.Infrastructure.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly DiContainer _container;
        private readonly WorldGenerator.Factory _worldGeneratorFactory;
        private readonly Tile.Factory _tileFactory;
        private readonly Building.Factory _buildingFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly SelectFrame.Factory _selectFrameFactory;
        private readonly UiCanvas.Factory _canvasFactory;
        private readonly Ground.Factory _groundFactory;

        public GameplayFactory(DiContainer container, WorldGenerator.Factory worldGeneratorFactory, Tile.Factory tileFactory, Building.Factory buildingFactory, IStaticDataService staticDataService, SelectFrame.Factory selectFrameFactory, UiCanvas.Factory canvasFactory, Ground.Factory groundFactory)
        {
            _container = container;
            _worldGeneratorFactory = worldGeneratorFactory;
            _tileFactory = tileFactory;
            _buildingFactory = buildingFactory;
            _staticDataService = staticDataService;
            _selectFrameFactory = selectFrameFactory;
            _canvasFactory = canvasFactory;
            _groundFactory = groundFactory;
        }

        public WorldGenerator WorldGenerator { get; private set; }

        public async UniTask<Ground> CreateGround(GroundType type, Vector3 position, GroundRotation rotation, Transform parent) =>
            await _groundFactory.Create(_staticDataService.GetGround(type).AssetReference, position, (int)rotation, parent);

        public async UniTask CreateCanvas()
        {
            UiCanvas canvas = await _canvasFactory.Create(GameplayFactoryAssets.Canvas);

            _container.Bind<DestroyBuildingButton>().FromInstance(canvas.GetComponentInChildren<DestroyBuildingButton>()).AsSingle();
            _container.Bind<ReplaceButton>().FromInstance(canvas.GetComponentInChildren<ReplaceButton>()).AsSingle();
        }

        public async UniTask CreateSelectFrame()
        {
            SelectFrame selectFrame = await _selectFrameFactory.Create(GameplayFactoryAssets.SelectFrame);

            _container.Bind<SelectFrame>().FromInstance(selectFrame).AsSingle();
        }

        public async UniTask CreateWorldGenerator()
        {
            WorldGenerator = await _worldGeneratorFactory.Create(GameplayFactoryAssets.WorldGenerator);

            _container.Bind<WorldGenerator>().FromInstance(WorldGenerator).AsSingle();
            _container.Bind<BuildingCreator>().FromInstance(WorldGenerator.GetComponent<BuildingCreator>()).AsSingle();
        }

        public async UniTask<Tile> CreateTile(Vector3 position, Transform parent)
        {
            Tile tile = await _tileFactory.Create(GameplayFactoryAssets.Tile, position, parent);

            return tile;
        }

        public async UniTask<Building> CreateBuilding(BuildingType type, Vector3 position, Transform parent)
        {
            MergeConfig mergeConfig = _staticDataService.GetMerge(type);
            Building building = await _buildingFactory.Create(mergeConfig.AssetReference, position, parent);

            building.Init(type);

            return building;
        }
    }
}
