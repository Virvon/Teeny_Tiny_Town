using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Zenject;

namespace Assets.Sources.Collection
{
    public class CollectionBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;
        private readonly IWorldFactory _worldFactory;
        private readonly IGameplayFactory _gameplayFactory;

        public CollectionBootstrapper(IUiFactory uiFactory, IWorldFactory worldFactory, IGameplayFactory gameplayFactory)
        {
            _uiFactory = uiFactory;
            _worldFactory = worldFactory;
            _gameplayFactory = gameplayFactory;
        }

        public async void Initialize()
        {
            await _gameplayFactory.CreatePlane();
            await _gameplayFactory.CreateUiSoundPlayer();
            await _worldFactory.CreateCollectionItemCreator();

            Window collectionWindow = await _uiFactory.CreateWindow(WindowType.Collection);

            collectionWindow.Open();
        }
    }
}
