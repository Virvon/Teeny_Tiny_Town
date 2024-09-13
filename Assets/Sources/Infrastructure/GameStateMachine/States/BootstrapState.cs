using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAssetProvider _assetProvider;

        public BootstrapState(GameStateMachine gameStateMachine, IAssetProvider assetProvider)
        {
            _gameStateMachine = gameStateMachine;
            _assetProvider = assetProvider;
        }

        public async UniTask Enter()
        {
            await Initialize();

            _gameStateMachine.Enter<GameLoopState>().Forget();
        }

        private async UniTask Initialize()
        {
            await _assetProvider.InitializeAsync();
        }

        public UniTask Exit() =>
            default;
    }
}
