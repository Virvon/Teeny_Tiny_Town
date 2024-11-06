using System;
using Assets.Sources.Infrastructure;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.UI.LoadingCurtain
{
    public class LoadingCurtainProxy : ILoadingCurtain
    {
        private readonly LoadingCurtain.Factory _factory;

        private ILoadingCurtain _implementation;

        public LoadingCurtainProxy(LoadingCurtain.Factory factory) =>
            _factory = factory;

        public async UniTask InitializeAsync() =>
            _implementation = await _factory.Create(InfrastructureAssetPath.LoadingCurtain);

        public void Show() =>
            _implementation.Show();

        public void Hide() =>
            _implementation.Hide();
    }
}
