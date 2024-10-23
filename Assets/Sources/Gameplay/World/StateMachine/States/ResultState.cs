using Assets.Sources.Data;
using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ResultState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;
        private readonly World _world;
        private readonly IWorldData _worldData;
        private readonly IWorldChanger _worldChanger;
        private readonly IStaticDataService _staticDataService;

        public ResultState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory, World world, IWorldData worldData, IWorldChanger worldChanger, IStaticDataService staticDataService)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
            _world = world;
            _worldData = worldData;
            _worldChanger = worldChanger;
            _staticDataService = staticDataService;
        }

        public async UniTask Enter()
        {
            _windowsSwitcher.Remove(WindowType.GameplayWindow);

            if (_windowsSwitcher.Contains(WindowType.ResultWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.ResultWindow);
                _windowsSwitcher.RegisterWindow(WindowType.ResultWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.ResultWindow);
            _world.StartRotating();
            _worldData.IsChangingStarted = false;
        }

        public UniTask Exit()
        {
            _world.TryStopRotating();

            _world.RotateToStart(callback: () =>
            {
                WorldConfig WorldConfig = _staticDataService.GetWorld<WorldConfig>(_worldData.Id);
                WorldData defaultWorldData = WorldConfig.GetWorldData();

                _worldData.Update(defaultWorldData.Tiles, defaultWorldData.NextBuildingTypeForCreation, defaultWorldData.AvailableBuildingsForCreation);
                _worldChanger.Update();
            });

            return default;
        }
    }
}
