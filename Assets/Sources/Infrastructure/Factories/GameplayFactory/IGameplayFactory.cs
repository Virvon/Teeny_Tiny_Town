using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.StaticDataService.Configs.Camera;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask<GameplayCamera> CreateCamera(GameplayCameraType type);
        UniTask<World> CreateWorld(string id, Vector3 position, Transform parent);
        UniTask<WorldsList> CreateWorldsList();
    }
}
