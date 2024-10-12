using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private LayerMask _actionHandlerLayerMask;

        private IPersistentProgressService _persistentPorgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService) =>
            _persistentPorgressService = persistentProgressService;

        public WorldStateMachine WorldStateMachine { get; private set; }

        public override void InstallBindings()
        {
            BindWorldBootstrapper();
            BindWorldChanger();
            BindActionHandlerLayerMask();
            BindActionHandlerStateMachine();
            BindWorldRepresentationChanger();
            BindWorldFactory();
            BindGameplayMover();
            BindWorldStateMachine();
            BindUiFactory();
            BindWorldData();
        }

        private void BindWorldData()
        {
            Container.BindInstance(_persistentPorgressService.Progress.CurrentWorldData).AsSingle();
        }

        private void BindUiFactory()
        {
            UiFactoryInstaller.Install(Container);
        }

        private void BindActionHandlerLayerMask()
        {
            Container.BindInstance(_actionHandlerLayerMask).AsSingle();
        }

        private void BindActionHandlerStateMachine()
        {
            ActionHandlerStateMachineInstaller.Install(Container);
        }

        private void BindWorldStateMachine()
        {
            WorldStateMachine = new();

            Container.BindInstance(WorldStateMachine).AsSingle();
            Container.Bind<StatesFactory>().AsSingle();
        }

        private void BindGameplayMover()
        {
            Container.BindInterfacesAndSelfTo<GameplayMover.GameplayMover>().AsSingle();
        }

        private void BindWorldFactory()
        {
            WorldFactoryInstaller.Install(Container);
        }

        private void BindWorldBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<WorldBootstrapper>().AsSingle().NonLazy();
        }

        private void BindWorldChanger()
        {
            Container.BindInterfacesAndSelfTo<WorldChanger>().AsSingle();
        }

        private void BindWorldRepresentationChanger()
        {
            Container.BindInterfacesAndSelfTo<WorldRepresentationChanger>().AsSingle();
        }
    }
}
