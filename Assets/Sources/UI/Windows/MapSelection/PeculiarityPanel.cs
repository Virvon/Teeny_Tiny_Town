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
        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;
        private IUiFactory _uiFactory;

        private List<PeculiarityIconPanel> _icons;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService,  IStaticDataService staticDataService, IUiFactory uiFactory)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;

            _icons = new();

            ChangeIcons();

            _persistentProgressService.Progress.CurrentWorldChanged += ChangeIcons;
        }

        private void OnDestroy() =>
            _persistentProgressService.Progress.CurrentWorldChanged -= ChangeIcons;

        private async void ChangeIcons()
        {
            foreach (PeculiarityIconPanel panel in _icons)
                Destroy(panel.gameObject);

            _icons.Clear();

            WorldConfig worldConfig = _staticDataService.GetWorld<WorldConfig>(_persistentProgressService.Progress.CurrentWorldData.Id);

            foreach (AssetReference iconAssetReference in worldConfig.PeculiarityIconAssetReferences)
                _icons.Add(await _uiFactory.CreatePeculiarityIconPanel(iconAssetReference, transform));
        }
    }
}