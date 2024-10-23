using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay.World.StateMachine
{
    public class WorldStateMachineInstaller : Installer<WorldStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<WorldStateMachine>().AsSingle();
            Container.Bind<StatesFactory>().AsSingle();
        }
    }
}
