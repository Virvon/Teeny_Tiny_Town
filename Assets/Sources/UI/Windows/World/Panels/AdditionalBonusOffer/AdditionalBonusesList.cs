using Assets.Sources.Data.World;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.AdditionalBonusOffer
{
    public class AdditionalBonusesList : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private IUiFactory _uiFactory;

        [Inject]
        private async void Construct(IStaticDataService staticDataService, IUiFactory uiFactory, IWorldData worldData)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;

            foreach (AdditionalBonusType additionalBonusType in _staticDataService.GetWorld<WorldConfig>(worldData.Id).AvailableAdditionalBonuses)
                await _uiFactory.CreateAdditionBonusOfferItem(additionalBonusType, transform);
        }
    }
}
