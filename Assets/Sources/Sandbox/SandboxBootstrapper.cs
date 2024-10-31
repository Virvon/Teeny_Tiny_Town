using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Zenject;

namespace Assets.Sources.Sandbox
{
    public class SandboxBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;
        private readonly SandboxChanger _sandboxChanger;
        private readonly IWorldFactory _worldFactory;

        public SandboxBootstrapper(IUiFactory uiFactory, SandboxChanger sandboxChanger, IWorldFactory worldFactory)
        {
            _uiFactory = uiFactory;
            _sandboxChanger = sandboxChanger;
            _worldFactory = worldFactory;
        }

        public async void Initialize()
        {
            Window sandboxWindow = await _uiFactory.CreateWindow(WindowType.Sandbox);
            WorldGenerator worldGenerator = await _worldFactory.CreateWorldGenerator();

            await _sandboxChanger.Generate(worldGenerator);

            sandboxWindow.Open();
        }
    }
}
