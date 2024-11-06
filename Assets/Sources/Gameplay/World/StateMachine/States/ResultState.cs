using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ResultState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly World _world;
        private readonly IWorldData _worldData;
        private readonly IWorldChanger _worldChanger;
        private readonly IStaticDataService _staticDataService;

        public ResultState(WindowsSwitcher windowsSwitcher, World world, IWorldData worldData, IWorldChanger worldChanger, IStaticDataService staticDataService)
        {
            _windowsSwitcher = windowsSwitcher;
            _world = world;
            _worldData = worldData;
            _worldChanger = worldChanger;
            _staticDataService = staticDataService;
        }

        public async UniTask Enter()
        {
            await _windowsSwitcher.Switch<ResultWindow>();
            _world.StartRotating();
            _worldData.IsChangingStarted = false;
        }

        public UniTask Exit()
        {
            _world.TryStopRotating();
            _world.RotateToStart(callback: _world.Clean);

            return default;
        }
    }
}
