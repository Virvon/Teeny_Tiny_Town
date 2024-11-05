using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StaticDataService;
using Zenject;

public class GameInstaller : MonoInstaller, ICoroutineRunner
{
    public override void InstallBindings()
    {
        BindSceneLoader();
        BindInputService();
        BindGameStateMachine();
        BindAssetProvider();
        BindStaticDataService();
        BindSaveLoadService();
        BindPersistentProgressService();
        BindCoroutineRunner();
    }

    private void BindCoroutineRunner()
    {
        Container.Bind<ICoroutineRunner>().FromInstance(this).AsSingle();
    }

    private void BindPersistentProgressService()
    {
        Container.BindInterfacesAndSelfTo<PersistentProgressService>().AsSingle();
    }

    private void BindSaveLoadService()
    {
        Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle();
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
