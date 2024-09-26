using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.WorldFactory
{
    public class WorldFactoryInstaller : Installer<WorldFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IWorldFactory>()
                .To<WorldFactory>()
                .AsSingle();

            Container
                .BindFactory<string, UniTask<WorldGenerator>, WorldGenerator.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<WorldGenerator>>();
        }
    }
}
