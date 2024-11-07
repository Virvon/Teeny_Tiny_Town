using Assets.Sources.Audio;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Services.ActivityTracking;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.UI.LoadingCurtain;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Assets.Sources.CompositionRoot
{
    public class GameInstaller : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private AudioMixer _audioMixer;

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
            BindAudioMixer();
            BindLoadingCurtainInstaller();
            BindActivityTraker();
        }

        private void BindActivityTraker()
        {
            Container.BindInterfacesAndSelfTo<ActivityTraker>().AsSingle().NonLazy();
        }

        private void BindLoadingCurtainInstaller()
        {
            LoadingCurtainIntaller.Install(Container);
        }

        private void BindAudioMixer()
        {
            Container.BindInstance(_audioMixer).AsSingle();
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
}