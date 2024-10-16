﻿using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.Root
{
    public class WorldBootstrapper : IInitializable
    {
        private readonly IWorldChanger _worldChanger;
        private readonly IWorldFactory _worldFactory;
        private readonly IWorldData _worldData;

        protected readonly WorldStateMachine WorldStateMachine;
        protected readonly StatesFactory StatesFactory;

        public WorldBootstrapper(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            IWorldData worldData)
        {
            _worldChanger = worldChanger;
            _worldFactory = worldFactory;
            WorldStateMachine = worldStateMachine;
            StatesFactory = statesFactory;
            _worldData = worldData;
        }

        public async void Initialize()
        {
            WorldGenerator worldGenerator = await _worldFactory.CreateWorldGenerator();

            worldGenerator.PlaceToCenter(_worldData.Length, _worldData.Width);
            await _worldChanger.Generate(worldGenerator);

            RegisterStates();
        }

        protected virtual void RegisterStates()
        {
            WorldStateMachine.RegisterState(StatesFactory.Create<WorldBootstrapState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<WorldChangingState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<ExitWorldState>());
        }
    }
}
