using Assets.Sources.Data;
using Assets.Sources.Gameplay.GameplayMover.Commands;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class ExpandingGameplayMover : GameplayMover, IExpandingGameplayMover
    {
        private readonly IExpandingWorldChanger _expandingWorldChanger;

        public ExpandingGameplayMover(
            IExpandingWorldChanger expandingWorldChanger,
            IInputService inputService,
            IWorldData worldData,
            IPersistentProgressService persistentProgressService)
            : base(expandingWorldChanger, inputService, worldData, persistentProgressService)
        {
            _expandingWorldChanger = expandingWorldChanger;
        }

        public void ExpandWorld(uint targetLength, uint targetWidth) =>
            ExecuteCommand(new ExpandWorldCommand(_expandingWorldChanger, WorldData, targetLength, targetWidth, LastCommand));
    }
}
