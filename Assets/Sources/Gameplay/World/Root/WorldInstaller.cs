﻿using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.PointsCounter;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.Windows;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.Root
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private LayerMask _actionHandlerLayerMask;
        [SerializeField] private World _world;

        private WorldsList _worldsList;

        protected string WorldDataId => _worldsList.CurrentWorldDataId ?? PersistentProgressService.Progress.LastPlayedWorldDataId;
        protected IPersistentProgressService PersistentProgressService { get; private set; }

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, WorldsList worldsList)
        {
            PersistentProgressService = persistentProgressService;
            _worldsList = worldsList;
        }

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
            BindNextBuildingForPlacingCreator();
            BindWorld();
            BindPointsCounter();
            BindRewardsCreator();
            BindQuestsChecker();
            BindWorldWindows();
            BindMarkersVisibility();
            BindRewarder();
            BindAcitonHandlerSwitcher();
            BindWorldCleaner();
        }

        protected virtual void BindAcitonHandlerSwitcher()
        {
            Container.BindInterfacesTo<ActionHandlerSwitcher>().AsSingle();
        }

        protected virtual void BindWorldWindows()
        {
            Container.BindInterfacesTo<WorldWindows>().AsSingle();
        }

        protected virtual void BindWorldBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<WorldBootstrapper>().AsSingle().NonLazy();
        }

        protected virtual void BindWorldData()
        {
            Container.BindInterfacesTo<WorldData>().FromInstance(PersistentProgressService.Progress.GetWorldData(WorldDataId)).AsSingle();
        }

        protected virtual void BindWorldChanger()
        {
            Container.BindInterfacesTo<WorldChanger>().AsSingle();
        }

        protected virtual void BindGameplayMover()
        {
            Container.BindInterfacesTo<GameplayMover.GameplayMover>().AsSingle();
        }

        private void BindWorldCleaner()
        {
            Container.BindInterfacesAndSelfTo<WorldCleaner>().AsSingle().NonLazy();
        }

        private void BindRewarder()
        {
            Container.Bind<Rewarder>().AsSingle();
        }

        private void BindMarkersVisibility()
        {
            Container.Bind<MarkersVisibility>().AsSingle();
        }

        private void BindQuestsChecker()
        {
            Container.BindInterfacesAndSelfTo<QuestsChecker>().AsSingle().NonLazy();
        }

        private void BindRewardsCreator()
        {
            Container.Bind<RewardsCreator>().AsSingle();
        }

        private void BindPointsCounter()
        {
            Container.BindInterfacesAndSelfTo<PointsCounter.PointsCounter>().AsSingle().NonLazy();
        }

        private void BindWorld()
        {
            Container.BindInterfacesAndSelfTo<World>().FromInstance(_world).AsSingle();
        }

        private void BindNextBuildingForPlacingCreator()
        {
            Container.Bind<NextBuildingForPlacingCreator>().AsSingle();
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
            WorldStateMachineInstaller.Install(Container);
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
