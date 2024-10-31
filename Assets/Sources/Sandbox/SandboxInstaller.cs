using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Zenject;

namespace Assets.Sources.Sandbox
{
    public class SandboxInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSandboxBootstrapper();
            BindUiFactory();
            WorldFactoryInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<SandboxChanger>().AsSingle();
            Container.BindInterfacesTo<SandboxRotation>().AsSingle();
        }

        private void BindUiFactory()
        {
            UiFactoryInstaller.Install(Container);
        }

        private void BindSandboxBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<SandboxBootstrapper>().AsSingle().NonLazy();
        }
    }
}
