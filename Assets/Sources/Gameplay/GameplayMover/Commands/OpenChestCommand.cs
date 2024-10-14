using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class OpenChestCommand : Command
    {
        private readonly uint _reward;
        private readonly Vector2Int _chestGridPosition;
        private readonly WorldWallet _worldWallet;

        public OpenChestCommand(IWorldChanger world, IWorldData worldData, uint reward, Vector2Int chestGridPosition, WorldWallet worldWallet)
            : base(world, worldData)
        {
            _reward = reward;
            _chestGridPosition = chestGridPosition;
            _worldWallet = worldWallet;
        }

        public override void Change()
        {
            _worldWallet.Give(_reward);

            WorldChanger.RemoveBuilding(_chestGridPosition);
        }

        public override UniTask Undo()
        {
            _worldWallet.ForceGet(_reward);

            return base.Undo();
        }
    }
}
