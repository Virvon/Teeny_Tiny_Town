using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public class GameplayFactoryInstaller : Installer<GameplayFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameplayFactory>()
                .To<GameplayFactory>()
                .AsSingle();

            Container
                .BindFactory<string, UniTask<WorldsList>, WorldsList.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<WorldsList>>();

            Container
               .BindFactory<string, Vector3, Transform, UniTask<World>, World.Factory>()
               .FromFactory<KeyPrefabFactoryAsync<World>>();

            Container
                .BindFactory<AssetReferenceGameObject, UniTask<GameplayCamera>, GameplayCamera.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<GameplayCamera>>();
        }
    }
}
