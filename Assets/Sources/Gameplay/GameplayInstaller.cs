using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.WorldGenerator.Comand;
using Assets.Sources.Gameplay.WorldGenerator.World;
using Assets.Sources.Infrastructure.GameplayFactory;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameplayBootstrapper();
            BindGameplayStateMachine();
            BindGameplayFactory();
            BindWorld();
            BindGameplayMover();
        }

        private void BindGameplayMover()
        {
            Container.BindInterfacesAndSelfTo<GameplayMover>().AsSingle();
        }

        private void BindWorld()
        {
            Container.BindInterfacesAndSelfTo<World>().AsSingle();
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
