using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Sandbox.ActionHandler;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class GroundsPanel : SandboxPanel
    {
        private GroundPositionHandler _groundPositionHaneler;

        private Dictionary<SandboxPanelElement, SandboxGroundType> _elements;
        private SandboxPanelElement _currentElement;

        [Inject]
        private async void Construct(IUiFactory uiFactory, IStaticDataService staticDataService, GroundPositionHandler groundPositionHandler)
        {
            _groundPositionHaneler = groundPositionHandler;

            _elements = new();

            foreach (SandboxGroundConfig groundConfig in staticDataService.SandboxConfig.Grounds)
            {
                SandboxPanelElement sandboxPanelElement = await uiFactory.CreateSandboxPanelElement(Content, groundConfig.IconAssetReference);
                _elements.Add(sandboxPanelElement, groundConfig.Type);

                sandboxPanelElement.Clicked += OnElementClicked;
            }

            _groundPositionHaneler.SetGround(_elements.Values.First());
            _currentElement = _elements.Keys.First();

            _currentElement.SetActive(true);
        }

        private void OnDestroy()
        {
            foreach (SandboxPanelElement sandboxPanelElement in _elements.Keys)
                sandboxPanelElement.Clicked -= OnElementClicked;
        }

        private void OnElementClicked(SandboxPanelElement element)
        {
            _currentElement.SetActive(false);
            _currentElement = element;
            _currentElement.SetActive(true);
            _groundPositionHaneler.SetGround(_elements[element]);
        }
    }
}
