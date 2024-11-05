using Assets.Sources.Data;
using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public abstract class Command
    {
        protected readonly IWorldChanger WorldChanger;
        protected readonly TileData[] TileDatas;
        protected readonly IWorldData WorldData;

        private readonly NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;
        private readonly BuildingsForPlacingData _buildingsForPlacingData;

        public Command(IWorldChanger worldChanger, IWorldData worldData, NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            WorldChanger = worldChanger;
            WorldData = worldData;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;

            BuildingsForPlacingData buildingsForPlacingData = _nextBuildingForPlacingCreator.BuildingsForPlacingData;

            _buildingsForPlacingData = new BuildingsForPlacingData(
                buildingsForPlacingData.StartGridPosition,
                buildingsForPlacingData.CurrentBuildingType,
                buildingsForPlacingData.NextBuildingType);

            TileDatas = WorldData.Tiles.Select(tile => new TileData(tile.GridPosition, tile.BuildingType)).ToArray();
        }

        public abstract void Execute();

        public virtual async UniTask Undo()
        {
            WorldData.UpdateTileDatas(TileDatas);
            await WorldChanger.Update(true);
            _nextBuildingForPlacingCreator.Update(_buildingsForPlacingData);
        }
    }
}
