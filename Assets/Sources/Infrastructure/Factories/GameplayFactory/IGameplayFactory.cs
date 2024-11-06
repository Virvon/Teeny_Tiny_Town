using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask<GameplayCamera> CreateCamera(Vector3 position);
        UniTask<World> CreateEducationWorld(Vector3 position, Transform parent);
        UniTask CreateUiSoundPlayer();
        UniTask<World> CreateWorld(string id, Vector3 position, Transform parent);
        UniTask<WorldsList> CreateWorldsList();
        UniTask CreateWorldWalletSoundPlayer();
    }
}
