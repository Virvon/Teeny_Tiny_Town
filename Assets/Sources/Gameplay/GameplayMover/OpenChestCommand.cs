using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class OpenChestCommand : Command
    {
        private readonly uint _reward;
        private readonly Vector2Int _chestGridPosition;
        private readonly WorldWallet _worldWallet;

        public OpenChestCommand(WorldChanger world, uint reward, Vector2Int chestGridPosition, WorldWallet worldWallet)
            : base(world)
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
