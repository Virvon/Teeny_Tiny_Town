using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StaticDataService;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindSceneLoader();
        BindInputService();
        BindGameStateMachine();
        BindAssetProvider();
        BindStaticDataService();
    }

    private void BindStaticDataService()
    {
        Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
    }

    private void BindAssetProvider()
    {
        Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
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
