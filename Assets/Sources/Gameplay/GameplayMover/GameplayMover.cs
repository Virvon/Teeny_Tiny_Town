using Assets.Sources.Gameplay.World.WorldInfrastructure;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class GameplayMover
    {
        private readonly World.WorldInfrastructure.World _world;

        private Command _lastCommand;

        public GameplayMover(World.WorldInfrastructure.World world) =>
            _world = world;

        public void PlaceNewBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new PlaceNewBuildingCommand(_world, gridPosition));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(_world, gridPosition));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType) =>
            ExecuteCommand(new ReplaceBuildingCommand(_world, fromGridPosition, fromBuildingType, toGridPosition, toBuildingType));

        public void TryUndoCommand()
        {
            if (_lastCommand == null)
                return;

            _world.Update(_lastCommand.TileDatas, _lastCommand.BuildingForPlacing);
            _lastCommand = null;
        }

        private void ExecuteCommand(Command command)
        {
            _lastCommand = command;
            command.Change();
        }
    }
}
