using Assets.Sources.Gameplay.WorldGenerator;
using Assets.Sources.Gameplay.WorldGenerator.World;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayLoopState : IState
    {
        private readonly WorldGenerator.WorldGenerator _worldGenerator;
        private readonly BuildingCreator _buildingCreator;
        private readonly World _world;

        public GameplayLoopState(WorldGenerator.WorldGenerator worldGenerator, BuildingCreator buildingCreator, World world)
        {
            _worldGenerator = worldGenerator;
            _buildingCreator = buildingCreator;
            _world = world;
        }

        public async UniTask Enter()
        {
            _world.Generate();
            await _worldGenerator.Generate();
            await _buildingCreator.Create();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
