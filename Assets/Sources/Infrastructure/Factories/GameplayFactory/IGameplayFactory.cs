using Assets.Sources.Gameplay.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask<World> CreateWorld(Vector3 position, Transform parent);
        UniTask<WorldsList> CreateWorldsList();
    }
}
