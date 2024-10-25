using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class ChangeBuildingForPlacingCommand : Command
    {
        private readonly BuildingType _targetBuilding;
        private readonly uint _buildingPrice;
        private readonly WorldWallet _worldWallet;
        private readonly NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;

        public ChangeBuildingForPlacingCommand(
            IWorldChanger world,
            IWorldData worldData,
            BuildingType targetBuilding,
            uint buildingPrice,
            WorldWallet worldWallet,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
            : base(world, worldData, nextBuildingForPlacingCreator)
        {
            _targetBuilding = targetBuilding;
            _buildingPrice = buildingPrice;
            _worldWallet = worldWallet;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
        }

        public override void Execute() =>
            _nextBuildingForPlacingCreator.ChangeCurrentBuildingForPlacing(_targetBuilding);

        public override UniTask Undo()
        {
            _worldWallet.Give(_buildingPrice);
            return base.Undo();
        }
    }
}
