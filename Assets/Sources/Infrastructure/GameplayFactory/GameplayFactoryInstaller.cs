using Assets.Sources.Gameplay.WorldGenerator;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Infrastructure.GameplayFactory
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
                .BindFactory<string, UniTask<WorldGenerator>, WorldGenerator.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<WorldGenerator>>();
        }
    }
}
