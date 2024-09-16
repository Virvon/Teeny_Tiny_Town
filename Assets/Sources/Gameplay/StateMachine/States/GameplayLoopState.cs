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
        private readonly World _world;

        public GameplayLoopState(WorldGenerator.WorldGenerator worldGenerator, World world)
        {
            _worldGenerator = worldGenerator;
            _world = world;
        }

        public async UniTask Enter()
        {
            _world.Generate();
            await _worldGenerator.Generate();
            _world.Work();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
