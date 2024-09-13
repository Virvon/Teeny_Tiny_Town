using Assets.Sources.Gameplay.WorldGenerator;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayLoopState : IState
    {
        private readonly WorldGenerator.WorldGenerator _worldGenerator;
        private readonly BuildingCreator _buildingCreator;

        public GameplayLoopState(WorldGenerator.WorldGenerator worldGenerator, BuildingCreator buildingCreator)
        {
            _worldGenerator = worldGenerator;
            _buildingCreator = buildingCreator;
        }

        public async UniTask Enter()
        {
            await _worldGenerator.Generate();
            await _buildingCreator.Create();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
