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
        private readonly World.WorldInfrastructure.WorldChanger _world;
        private readonly WindowsSwitcher _windowsSwithcer;

        public GameplayLoopState(WindowsSwitcher windowsSwithcer)
        {
            _windowsSwithcer = windowsSwithcer;
        }

        public async UniTask Enter()
        {
            _world.Generate();
            await _worldGenerator.Generate();
            _world.Work();
            
            _windowsSwithcer.Switch(WindowType.GameplayWindow);
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
