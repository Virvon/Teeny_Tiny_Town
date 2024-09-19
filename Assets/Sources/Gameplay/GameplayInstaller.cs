using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.GameplayFactory;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private LayerMask _actionHandlerLayerMask;

        public override void InstallBindings()
        {
            BindGameplayBootstrapper();
            BindGameplayStateMachine();
            BindGameplayFactory();
            BindWorld();
            BindGameplayMover();
            BindActionHandlerStateMachine();
            BindActionHandlerLayerMask();
            BindWorldRepresentationChanger();
        }

        private void BindWorldRepresentationChanger()
        {
            Container.BindInterfacesAndSelfTo<WorldRepresentationChanger>().AsSingle();
        }

        private void BindActionHandlerLayerMask()
        {
            Container.BindInstance(_actionHandlerLayerMask).AsSingle();
        }

        private void BindActionHandlerStateMachine()
        {
            ActionHandlerStateMachineInstaller.Install(Container);
        }

        private void BindGameplayMover()
        {
            Container.BindInterfacesAndSelfTo<GameplayMover.GameplayMover>().AsSingle();
        }

        private void BindWorld()
        {
            Container.BindInterfacesAndSelfTo<World.WorldInfrastructure.World>().AsSingle();
        }

        private void BindGameplayFactory()
        {
            GameplayFactoryInstaller.Install(Container);
        }

        private void BindGameplayBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<GameplayBootstrapper>().AsSingle().NonLazy();
        }

        private void BindGameplayStateMachine()
        {
            GameplayStateMachineInstaller.Install(Container);
        }
    }
}
