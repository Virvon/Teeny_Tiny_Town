using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StateMachine;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.Root
{
    public class CurrencyWorldBootstrapper : WorldBootstrapper
    {
        public CurrencyWorldBootstrapper(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            IWorldData worldData)
            : base(worldChanger, worldFactory, worldStateMachine, statesFactory, worldData)
        {
        }

        protected override void RegisterStates()
        {
            WorldStateMachine.RegisterState(StatesFactory.Create<CurrencyWorldBootstrapState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<CurrencyWorldChangingState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<StoreState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<ExitWorldState>());
        }
    }
}
