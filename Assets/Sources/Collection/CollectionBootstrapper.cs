using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Zenject;

namespace Assets.Sources.Collection
{
    public class CollectionBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;

        public CollectionBootstrapper(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async void Initialize()
        {
            Window collectionWindow = await _uiFactory.CreateWindow(WindowType.Collection);

            collectionWindow.Open();
        }
    }
}
