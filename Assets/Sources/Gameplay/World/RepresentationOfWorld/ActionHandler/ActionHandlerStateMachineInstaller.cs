using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ActionHandlerStateMachineInstaller : Installer<ActionHandlerStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ActionHandlerStateMachine>().AsSingle();
            Container.Bind<ActionHandlerStatesFactory>().AsSingle();
        }
    }
}