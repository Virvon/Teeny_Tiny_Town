﻿using System.Collections.Generic;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
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

            _icons = new ();

            ChangeIcons(_worldsList.CurrentWorldDataId);

            _worldsList.CurrentWorldChanged += ChangeIcons;
        }

        private void OnDestroy() =>
            _worldsList.CurrentWorldChanged += ChangeIcons;

        private async void ChangeIcons(string worldDataId)
        {
            foreach (PeculiarityIconPanel panel in _icons)
                Destroy(panel.gameObject);

            _icons.Clear();

            WorldConfig worldConfig = _staticDataService.GetWorld<WorldConfig>(worldDataId);

            foreach (AssetReference iconAssetReference in worldConfig.PeculiarityIconAssetReferences)
                _icons.Add(await _uiFactory.CreatePeculiarityIconPanel(iconAssetReference, transform));
        }
    }
}