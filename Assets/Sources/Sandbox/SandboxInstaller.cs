using Assets.Sources.Infrastructure.Factories.UiFactory;
using Zenject;

namespace Assets.Sources.Sandbox
{
    public class SandboxInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSandboxBootstrapper();
            BindUiFactory();
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
