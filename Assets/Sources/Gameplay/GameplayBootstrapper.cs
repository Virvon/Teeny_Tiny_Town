using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly CamerasSwitcher _camerasSwitcher;

        public GameplayBootstrapper(
            GameplayStateMachine gameplayStateMachine,
            StatesFactory statesFactory,
            IGameplayFactory gameplayFactory,
            CamerasSwitcher camerasSwitcher)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _statesFactory = statesFactory;
            _gameplayFactory = gameplayFactory;
            _camerasSwitcher = camerasSwitcher;
        }

        public async void Initialize()
        {          
            WorldsList worldsList = await _gameplayFactory.CreateWorldsList();
            await _camerasSwitcher.CreateCameras();

            RegisterGameplayStates();

            await worldsList.Create();

            await _gameplayStateMachine.Enter<GameplayStartState>();
        }

        private void RegisterGameplayStates()
        {
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayLoopState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayStartState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<MapSelectionState>());
        }

        
    }
}
