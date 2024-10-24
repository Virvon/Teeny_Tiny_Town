using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.UI;
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
            BindWindowsSwitcher();
            BindUiFactory();
        }

        private void BindUiFactory()
        {
            UiFactoryInstaller.Install(Container);
        }

        private void BindWindowsSwitcher()
        {
            Container.BindInterfacesAndSelfTo<WindowsSwitcher>().AsSingle();
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
