using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayLoopState : IState
    {
        private readonly World.RepresentationOfWorld.WorldGenerator _worldGenerator;
        private readonly World.WorldInfrastructure.World _world;

        public GameplayLoopState(World.RepresentationOfWorld.WorldGenerator worldGenerator, World.WorldInfrastructure.World world)
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
