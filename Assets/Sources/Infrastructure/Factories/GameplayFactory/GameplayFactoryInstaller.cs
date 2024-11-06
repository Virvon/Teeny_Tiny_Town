using Assets.Sources.Audio;
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
               .BindFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<World>, World.Factory>()
               .FromFactory<ReferencePrefabFactoryAsync<World>>();

            Container
                .BindFactory<string, UniTask<GameplayCamera>, GameplayCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<GameplayCamera>>();
            
            Container
                .BindFactory<string, UniTask<UiSoundPlayer>, UiSoundPlayer.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<UiSoundPlayer>>();
            
            Container
                .BindFactory<string, UniTask<WorldWalletSoundPlayer>, WorldWalletSoundPlayer.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<WorldWalletSoundPlayer>>();
        }
    }
}
