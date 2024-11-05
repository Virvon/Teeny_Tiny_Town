using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.UI.Windows.MapSelection
{
    public class PeculiarityPanel : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private IUiFactory _uiFactory;
        private WorldsList _worldsList;
        private List<PeculiarityIconPanel> _icons;

        [Inject]
        private void Construct(
            IStaticDataService staticDataService,
            IUiFactory uiFactory,
            WorldsList worldsList)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _worldsList = worldsList;

            _icons = new();

            ChangeIcons(_worldsList.CurrentWorldData);

            _worldsList.CurrentWorldChanged += ChangeIcons;
        }

        private void OnDestroy() =>
            _worldsList.CurrentWorldChanged += ChangeIcons;

        private async void ChangeIcons(IWorldData worldData)
        {
            foreach (PeculiarityIconPanel panel in _icons)
                Destroy(panel.gameObject);

            _icons.Clear();

            WorldConfig worldConfig = _staticDataService.GetWorld<WorldConfig>(worldData.Id);

            foreach (AssetReference iconAssetReference in worldConfig.PeculiarityIconAssetReferences)
                _icons.Add(await _uiFactory.CreatePeculiarityIconPanel(iconAssetReference, transform));
        }
    }
}