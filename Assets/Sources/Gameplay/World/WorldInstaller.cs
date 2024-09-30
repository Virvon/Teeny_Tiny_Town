using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private LayerMask _actionHandlerLayerMask;
        [SerializeField] private World _world;

        public WorldStateMachine WorldStateMachine { get; private set; }

        public override void InstallBindings()
        {
            BindWorldBootstrapper();
            BindWorldChanger();
            BindActionHandlerLayerMask();
            BindActionHandlerStateMachine();
            BindWorldRepresentationChanger();
            BindWorldFactory();
            BindGameplayMover();
            BindWorldStateMachine();
            BindMoveCounter();
            BindUiFactory();
            BindWorld();
        }

        private void BindWorld()
        {
            Container.BindInstance(_world).AsSingle();
        }

        private void BindUiFactory()
        {
            UiFactoryInstaller.Install(Container);
        }

        private void BindActionHandlerLayerMask()
        {
            Container.BindInstance(_actionHandlerLayerMask).AsSingle();
        }

        private void BindActionHandlerStateMachine()
        {
            ActionHandlerStateMachineInstaller.Install(Container);
        }

        private void BindMoveCounter()
        {
            Container.BindInterfacesAndSelfTo<MoveCounter>().AsSingle();
        }

        private void BindWorldStateMachine()
        {
            WorldStateMachineInstaller.Install(Container);
            WorldStateMachine = Container.Resolve<WorldStateMachine>();
        }

        private void BindGameplayMover()
        {
            Container.BindInterfacesAndSelfTo<GameplayMover.GameplayMover>().AsSingle();
        }

        private void BindWorldFactory()
        {
            WorldFactoryInstaller.Install(Container);
        }

        private void BindWorldBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<WorldBootstrapper>().AsSingle().NonLazy();
        }

        private void BindWorldChanger()
        {
            Container.BindInterfacesAndSelfTo<WorldChanger>().AsSingle();
        }

        private void BindWorldRepresentationChanger()
        {
            Container.BindInterfacesAndSelfTo<WorldRepresentationChanger>().AsSingle();
        }
    }
}
