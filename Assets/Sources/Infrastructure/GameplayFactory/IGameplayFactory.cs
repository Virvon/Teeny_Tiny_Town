using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Tile = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Tile;

namespace Assets.Sources.Infrastructure.GameplayFactory
{
    public interface IGameplayFactory
    {
        WorldGenerator WorldGenerator { get; }

        UniTask<Gameplay.World.RepresentationOfWorld.Tiles.Building> CreateBuilding(BuildingType type, Vector3 position, Transform parent);
        UniTask CreateCanvas();
        UniTask CreateSelectFrame();
        UniTask<Tile> CreateTile(Vector3 position, Transform parent);
        UniTask CreateWorldGenerator();
    }
}
