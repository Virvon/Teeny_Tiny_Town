﻿using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Sandbox.ActionHandler;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Zenject;

namespace Assets.Sources.Sandbox
{
    public class SandboxBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;
        private readonly SandboxChanger _sandboxChanger;
        private readonly IWorldFactory _worldFactory;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly ActionHandlerStatesFactory _actionHandlerStatesFactory;
        private readonly SandboxRotation _sandboxRotation;

        public SandboxBootstrapper(
            IUiFactory uiFactory,
            SandboxChanger sandboxChanger,
            IWorldFactory worldFactory,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            SandboxRotation sandboxRotation)
        {
            _uiFactory = uiFactory;
            _sandboxChanger = sandboxChanger;
            _worldFactory = worldFactory;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
            _sandboxRotation = sandboxRotation;
        }

        public async void Initialize()
        {
            SandboxWorld sandboxWorld = await _worldFactory.CreateSandboxWorld();
            WorldGenerator worldGenerator = await _worldFactory.CreateWorldGenerator(sandboxWorld.transform);

            _sandboxRotation.Init(sandboxWorld);

            await _sandboxChanger.Generate(worldGenerator);
            await _worldFactory.CreateSelectFrame(worldGenerator.transform);

            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<BuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<GroundPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ClearTilePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NothingSelectedState>());

            Window sandboxWindow = await _uiFactory.CreateWindow(WindowType.Sandbox);

            _actionHandlerStateMachine.Enter<NothingSelectedState>();

            sandboxWindow.Open();
        }
    }
}
