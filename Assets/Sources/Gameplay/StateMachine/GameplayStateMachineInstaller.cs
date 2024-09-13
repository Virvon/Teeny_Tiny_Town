using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay.StateMachine
{
    public class GameplayStateMachineInstaller : Installer<GameplayStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<StatesFactory>().AsSingle();
            Container.Bind<GameplayStateMachine>().AsSingle();
        }
    }
}
