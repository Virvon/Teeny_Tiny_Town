using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TileRepresentation = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.TileRepresentation;

namespace Assets.Sources.Infrastructure.GameplayFactory
{
    public interface IGameplayFactory
    {
        WorldGenerator WorldGenerator { get; }

        UniTask<Gameplay.World.RepresentationOfWorld.Tiles.Building> CreateBuilding(BuildingType type, Vector3 position, Transform parent);
        UniTask CreateBuildingMarker();
        UniTask CreateCanvas();
        UniTask<Gameplay.World.RepresentationOfWorld.Tiles.Grounds.Ground> CreateGround(GroundType type, Vector3 position, GroundRotation rotation, Transform parent);
        UniTask CreateSelectFrame();
        UniTask<TileRepresentation> CreateTile(Vector3 position, Transform parent);
        UniTask CreateWorldGenerator();
    }
}
