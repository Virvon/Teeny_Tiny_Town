using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;
using System.Linq;

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

        public UniTask Enter()
        {
            _windowsSwitcher.Switch<ResultWindow>("result state");
            _world.StartRotating();
            _worldData.IsChangingStarted = false;

            return default;
        }

        public UniTask Exit()
        {
            _world.TryStopRotating();

            _world.RotateToStart(callback: () =>
            {
                WorldConfig WorldConfig = _staticDataService.GetWorld<WorldConfig>(_worldData.Id);

                _worldData.Update(WorldConfig.TilesDatas, WorldConfig.NextBuildingTypeForCreation, WorldConfig.StartingAvailableBuildingTypes.ToList());
                _worldChanger.Update(true);
            });

            return default;
        }
    }
}
