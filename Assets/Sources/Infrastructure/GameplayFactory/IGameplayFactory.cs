using Assets.Sources.Gameplay.Tile;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask<Building> CreateBuilding(Vector3 position, Transform parent);
        UniTask<Tile> CreateTile(Vector3 position, Transform parent);
        UniTask CreateWorldGenerator();
    }
}
