using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayLoopState : IState
    {
        private readonly WorldGenerator _worldGenerator;
        private readonly World.WorldInfrastructure.World _world;
        private readonly WindowsSwitcher _windowsSwithcer;

        public GameplayLoopState(WorldGenerator worldGenerator, World.WorldInfrastructure.World world, WindowsSwitcher windowsSwithcer)
        {
            _worldGenerator = worldGenerator;
            _world = world;
            _windowsSwithcer = windowsSwithcer;
        }

        public async UniTask Enter()
        {
            _world.Generate();
            await _worldGenerator.Generate();
            _world.Work();
            await _windowsSwithcer.CreateWindows();
            _windowsSwithcer.Switch(WindowType.GameplayWindow);
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
