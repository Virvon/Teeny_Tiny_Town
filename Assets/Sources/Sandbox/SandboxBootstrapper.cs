using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Zenject;

namespace Assets.Sources.Sandbox
{
    public class SandboxBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;

        public SandboxBootstrapper(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async void Initialize()
        {
            Window sandboxWindow = await _uiFactory.CreateWindow(WindowType.Sandbox);

            sandboxWindow.Open();
        }
    }
}
