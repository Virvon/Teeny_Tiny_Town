using Assets.Sources.Data;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;

        public LoadProgressState(IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService, GameStateMachine gameStateMachine, IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
        }

        public UniTask Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<GameLoopState>().Forget();

            return default;
        }

        public UniTask Exit() =>
            default;

        private void LoadProgressOrInitNew() =>
            _persistentProgressService.Progress = _saveLoadService.LoadProgress() ?? CreateNewProgress();

        private PlayerProgress CreateNewProgress()
        {
            PlayerProgress progress = new PlayerProgress(GetWorldDatas());

            return progress;
        }

        private WorldData[] GetWorldDatas()
        {
            ReadOnlyArray<WorldConfig> worldConfigs = _staticDataService.WorldConfigs;

            WorldData[] worldDatas = new WorldData[worldConfigs.Count];

            for(int i = 0; i < worldConfigs.Count; i++)
            {
                WorldConfig worldConfig = worldConfigs[i];

                worldDatas[i] = worldConfig.GetWorldData();
                worldDatas[i].WorldWallet.Value = 3000;
            }

            return worldDatas;
        }
    }
}
