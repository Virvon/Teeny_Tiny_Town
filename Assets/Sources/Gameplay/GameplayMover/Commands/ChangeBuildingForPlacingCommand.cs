using Assets.Sources.Data.WorldDatas;
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

        public ChangeBuildingForPlacingCommand(IWorldChanger world, IWorldData worldData, BuildingType targetBuilding, uint buildingPrice, WorldWallet worldWallet)
            : base(world, worldData)
        {
            _targetBuilding = targetBuilding;
            _buildingPrice = buildingPrice;
            _worldWallet = worldWallet;
        }

        public override void Change() =>
            WorldChanger.ChangeBuildingForPlacing(_targetBuilding);

        public override UniTask Undo()
        {
            _worldWallet.Give(_buildingPrice);
            return base.Undo();
        }
    }
}
