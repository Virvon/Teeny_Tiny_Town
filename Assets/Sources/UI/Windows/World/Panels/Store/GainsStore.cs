using Assets.Sources.Data.World.Currency;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class GainsStore : MonoBehaviour
    {
        private ICurrencyWorldData _currencyWorldData;
        private IStaticDataService _staticDataService;

        [Inject]
        private async void Construct(ICurrencyWorldData currencyWorldData, IStaticDataService staticDataService, IUiFactory uiFactory)
        {
            _currencyWorldData = currencyWorldData;
            _staticDataService = staticDataService;

            foreach (GainStoreItemType gainType in _staticDataService.GetWorld<CurrencyWorldConfig>(_currencyWorldData.Id).AvailableGainStoreItems)
                await uiFactory.CreateGainStoreItemPanel(gainType, transform);
        }
    }
}
