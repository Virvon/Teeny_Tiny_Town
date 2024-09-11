using Assets.Sources.Infrastructure;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.SceneManagment;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindSceneLoader();
        BindInputService();
        BindGameStateMachine();
    }

    private void BindInputService()
    {
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
    }

    private void BindSceneLoader()
    {
        Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
    }

    private void BindGameStateMachine()
    {
        GameStateMachineInstaller.Install(Container);
    }
}
