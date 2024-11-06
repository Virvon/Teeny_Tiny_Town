using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Sandbox;
using Zenject;

namespace Assets.Sources.Collection
{
    public class CollectionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CollectionBootstrapper>().AsSingle().NonLazy();
            UiFactoryInstaller.Install(Container);
            WorldFactoryInstaller.Install(Container);
            Container.BindInterfacesTo<CollectionRotation>().AsSingle();
        }
    }
}
