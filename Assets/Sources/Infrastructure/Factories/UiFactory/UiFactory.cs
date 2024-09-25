using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactory : IUiFactory
    {
        private readonly DiContainer _container;
        private readonly Window.Factory _windowFactory;
        private readonly IStaticDataService _staticDataService;

        public UiFactory(DiContainer container, Window.Factory windowFactory, IStaticDataService staticDataService)
        {
            _container = container;
            _windowFactory = windowFactory;
            _staticDataService = staticDataService;
        }

        public async UniTask<Window> CreateWindow(WindowType type)
        {
            Window window = await _windowFactory.Create(_staticDataService.GetWindow(type).AssetReference);

            return window;
        }
    }
}
