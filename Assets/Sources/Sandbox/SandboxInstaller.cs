using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Sandbox.ActionHandler;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Sandbox
{
    public class SandboxInstaller : MonoInstaller
    {
        [SerializeField] private LayerMask _layerMask;

        public override void InstallBindings()
        {
            BindSandboxBootstrapper();
            BindUiFactory();
            WorldFactoryInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<SandboxChanger>().AsSingle();
            Container.BindInterfacesTo<SandboxRotation>().AsSingle();
            ActionHandlerStateMachineInstaller.Install(Container);
            Container.BindInstance(_layerMask);
            Container.Bind<SandboxActionHandlerSwitcher>().AsSingle().NonLazy();
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
