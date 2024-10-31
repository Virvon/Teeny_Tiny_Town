using Assets.Sources.Infrastructure.Factories.UiFactory;
using Zenject;

namespace Assets.Sources.Collection
{
    public class CollectionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CollectionBootstrapper>().AsSingle().NonLazy();
            UiFactoryInstaller.Install(Container);
        }
    }
}
