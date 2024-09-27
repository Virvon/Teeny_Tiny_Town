using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay.World.StateMachine
{
    public class WorldStateMachineInstaller : Installer<WorldStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<StatesFactory>().AsSingle();
            Container.Bind<WorldStateMachine>().AsSingle();
        }
    }
}
