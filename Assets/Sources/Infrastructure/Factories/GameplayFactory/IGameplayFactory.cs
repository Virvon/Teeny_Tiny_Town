using Assets.Sources.Gameplay.World;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TileRepresentation = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.TileRepresentation;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask<Gameplay.World.RepresentationOfWorld.Tiles.Building> CreateBuilding(BuildingType type, Vector3 position, Transform parent);
        UniTask CreateBuildingMarker();
        UniTask<Gameplay.World.RepresentationOfWorld.Tiles.Grounds.Ground> CreateGround(GroundType groundType, RoadType roadType, Vector3 position, GroundRotation rotation, Transform parent);
        UniTask CreateSelectFrame();
        UniTask<TileRepresentation> CreateTile(Vector3 position, Transform parent);
        UniTask CreateWorld(Vector3 position, Transform parent);
        UniTask<WorldsList> CreateWorldsList();
    }
}
