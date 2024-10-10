using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
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

        UniTask<Gameplay.World.RepresentationOfWorld.Tiles.Buildings.BuildingRepresentation> CreateBuilding(BuildingType type, Vector3 position, Transform parent);
        UniTask CreateBuildingMarker();
        UniTask<Gameplay.World.RepresentationOfWorld.Tiles.Grounds.Ground> CreateGround(TileType tileType, Vector3 position, Transform parent);
        UniTask<Gameplay.World.RepresentationOfWorld.Tiles.Grounds.Ground> CreateGround(GroundType groundType, RoadType roadType, Vector3 position, GroundRotation rotation, Transform parent);
        UniTask CreateSelectFrame();
        UniTask<TileRepresentation> CreateTileRepresentation(Vector3 position, Transform parent);
        UniTask<WorldGenerator> CreateWorldGenerator(Transform parent);
    }
}