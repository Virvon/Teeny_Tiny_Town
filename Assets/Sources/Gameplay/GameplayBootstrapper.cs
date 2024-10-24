using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows;
using Assets.Sources.UI.Windows.Start;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;

        public GameplayBootstrapper(
            GameplayStateMachine gameplayStateMachine,
            StatesFactory statesFactory,
            IGameplayFactory gameplayFactory,
            WindowsSwitcher windowsSwitcher,
            IUiFactory uiFactory)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _statesFactory = statesFactory;
            _gameplayFactory = gameplayFactory;
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
        }

        public async void Initialize()
        {
            await _gameplayFactory.CreateCamera();
            WorldsList worldsList = await _gameplayFactory.CreateWorldsList();

            RegisterGameplayStates();

            await worldsList.CreateCurrentWorld();
            await RegisterWindows();

            await _gameplayStateMachine.Enter<GameStartState>();
        }

        private void RegisterGameplayStates()
        {
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayLoopState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameStartState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<MapSelectionState>());
        }  

        private async UniTask RegisterWindows()
        {
            await _windowsSwitcher.RegisterWindow<MapSelectionWindow>(WindowType.MapSelectionWindow, _uiFactory);
            await _windowsSwitcher.RegisterWindow<StartWindow>(WindowType.StartWindow, _uiFactory);
        }
    }
}
