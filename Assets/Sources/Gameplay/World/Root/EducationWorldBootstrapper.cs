using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;

namespace Assets.Sources.Gameplay.World.Root
{
    public class EducationWorldBootstrapper : CurrencyWorldBootstrapper
    {
        private const uint GainItemsCount = 1;

        private readonly IWorldData _worldData;

        public EducationWorldBootstrapper(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            World world,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService,
            IWorldData worldData)
            : base(worldChanger, worldFactory, worldStateMachine, statesFactory, world, actionHandlerStateMachine, actionHandlerStatesFactory, nextBuildingForPlacingCreator, persistentProgressService)
        {
            _worldData = worldData;
        }

        public override void Initialize()
        {
            _worldData.ReplaceItems.Count = GainItemsCount;
            _worldData.BulldozerItems.Count = GainItemsCount;
            _worldData.IsChangingStarted = true;

            base.Initialize();
        }
    }
}
