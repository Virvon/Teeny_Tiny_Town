﻿using System;
using System.Collections;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.UI.LoadingCurtain;
using Cysharp.Threading.Tasks;
#if !UNITY_WEBGL || UNITY_EDITOR
using Agava.YandexGames;
#endif

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingCurtainProxy _loadingCurtainProxy;

        public BootstrapState(
            GameStateMachine gameStateMachine,
            IAssetProvider assetProvider,
            IStaticDataService staticDataService,
            ICoroutineRunner coroutineRunner,
            LoadingCurtainProxy loadingCurtainProxy)
        {
            _gameStateMachine = gameStateMachine;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
            _loadingCurtainProxy = loadingCurtainProxy;
        }

        public async UniTask Enter()
        {
            await Initialize();

            _coroutineRunner.StartCoroutine(InitializeYandexSdk(callback: () => _gameStateMachine.Enter<LoadProgressState>().Forget()));
        }

        private async UniTask Initialize()
        {
            await _assetProvider.InitializeAsync();
            await _staticDataService.InitializeAsync();
            await _loadingCurtainProxy.InitializeAsync();

            _loadingCurtainProxy.Show();
        }

        public UniTask Exit() =>
            default;

        private IEnumerator InitializeYandexSdk(Action callback)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            callback?.Invoke();
            yield break;
#else
            yield return YandexGamesSdk.Initialize();

            if (YandexGamesSdk.IsInitialized == false)
                throw new ArgumentNullException(nameof(YandexGamesSdk), "Yandex SDK didn't initialized correctly");

            YandexGamesSdk.CallbackLogging = true;
            StickyAd.Show();
            callback?.Invoke();
#endif
        }
    }
}
