using Assets.Sources.Data.WorldDatas.Currency;
using Assets.Sources.Gameplay.GameplayMover.Commands;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class ExpandingGameplayMover : CurrencyGameplayMover, IExpandingGameplayMover
    {
        private readonly IExpandingWorldChanger _expandingWorldChanger;

        public ExpandingGameplayMover(
            IExpandingWorldChanger expandingWorldChanger,
            IInputService inputService,
            ICurrencyWorldData worldData,
            IPersistentProgressService persistentProgressService,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
            : base(expandingWorldChanger, inputService, worldData, persistentProgressService, nextBuildingForPlacingCreator)
        {
            _expandingWorldChanger = expandingWorldChanger;
        }

        public void ExpandWorld(Vector2Int targetSize) =>
            ExecuteCommand(new ExpandWorldCommand(_expandingWorldChanger, WorldData, targetSize, LastCommand, NextBuildingForPlacingCreator));
    }
}
