using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds;
using Assets.Sources.Sandbox;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.WorldFactory
{
    public interface IWorldFactory
    {
        WorldGenerator WorldGenerator { get; }

        UniTask<BuildingRepresentation> CreateBuilding(BuildingType type, Vector3 position, Transform parent);
        UniTask CreateBuildingMarker(Transform parent);
        UniTask CreateCollectionItemCreator();
        UniTask<Ground> CreateGround(TileType tileType, Vector3 position, Transform parent);
        UniTask<Ground> CreateGround(GroundType groundType, RoadType roadType, Vector3 position, GroundRotation rotation, Transform parent);
        UniTask<SandboxWorld> CreateSandboxWorld();
        UniTask<SelectFrame> CreateSelectFrame(Transform parent);
        UniTask<TileRepresentation> CreateTileRepresentation(Vector3 position, Transform parent);
        UniTask<WorldGenerator> CreateWorldGenerator(Transform parent = null);
    }
}