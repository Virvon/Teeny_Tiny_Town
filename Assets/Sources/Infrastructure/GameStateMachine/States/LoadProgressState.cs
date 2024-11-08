using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Data;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.Utilities;
using Assets.Sources.Data.World;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private const uint StarGamplayWalletValue = 100;

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
            List<QuestData> startQuests = new ();
            startQuests.AddRange(_staticDataService.QuestsConfig.StartQuestsId.Select(id => new QuestData(id)));

            PlayerProgress progress = new PlayerProgress(
                GetWorldDatas(),
                startQuests,
                _staticDataService.WorldsConfig.AvailableMovesCount,
                _staticDataService.SandboxConfig.Size,
                _staticDataService.GetAllBuildings(),
                _staticDataService.WorldsConfig.EducationWorldId,
                StarGamplayWalletValue);

            return progress;
        }

        private WorldData[] GetWorldDatas()
        {
            ReadOnlyArray<WorldConfig> worldConfigs = _staticDataService.WorldConfigs;

            WorldData[] worldDatas = new WorldData[worldConfigs.Count];

            for (int i = 0; i < worldConfigs.Count; i++)
            {
                WorldConfig worldConfig = worldConfigs[i];

                worldDatas[i] = worldConfig.GetWorldData(_staticDataService.WorldsConfig.Goals, _staticDataService);
            }

            return worldDatas;
        }
    }
}
