﻿using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.Root
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private LayerMask _actionHandlerLayerMask;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService) =>
            _persistentProgressService = persistentProgressService;

        public WorldStateMachine WorldStateMachine { get; private set; }
        protected WorldData WorldData => _persistentProgressService.Progress.CurrentWorldData;

        public override void InstallBindings()
        {
            BindWorldBootstrapper();
            BindWorldData();
            BindWorldChanger();
            BindActionHandlerLayerMask();
            BindActionHandlerStateMachine();
            BindWorldRepresentationChanger();
            BindWorldFactory();
            BindGameplayMover();
            BindWorldStateMachine();
            BindUiFactory();
        }

        protected virtual void BindWorldBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<WorldBootstrapper>().AsSingle().NonLazy();
        }

        protected virtual void BindWorldData()
        {
            Container.BindInterfacesTo<WorldData>().FromInstance(WorldData).AsSingle();
        }

        protected virtual void BindWorldChanger()
        {
            Container.BindInterfacesTo<WorldChanger>().AsSingle();
        }

        protected virtual void BindGameplayMover()
        {
            Container.BindInterfacesTo<GameplayMover.GameplayMover>().AsSingle();
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

        private void BindWorldFactory()
        {
            WorldFactoryInstaller.Install(Container);
        }

        private void BindWorldRepresentationChanger()
        {
            Container.BindInterfacesAndSelfTo<WorldRepresentationChanger>().AsSingle();
        }
    }
}
