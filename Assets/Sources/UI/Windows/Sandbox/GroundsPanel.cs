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

        [Inject]
        private async void Construct(IUiFactory uiFactory, IStaticDataService staticDataService, GroundPositionHandler groundPositionHandler)
        {
            _groundPositionHaneler = groundPositionHandler;

            _elements = new();

            foreach (SandboxGroundType sandboxGroundType in staticDataService.SandboxConfig.Grounds)
            {
                SandboxPanelElement sandboxPanelElement = await uiFactory.CreateSandboxPanelElement(sandboxGroundType, Content);
                _elements.Add(sandboxPanelElement, sandboxGroundType);

                sandboxPanelElement.Clicked += OnElementClicked;
            }

            _groundPositionHaneler.SetGround(_elements.Values.First());
        }

        private void OnDestroy()
        {
            foreach (SandboxPanelElement sandboxPanelElement in _elements.Keys)
                sandboxPanelElement.Clicked -= OnElementClicked;
        }

        private void OnElementClicked(SandboxPanelElement element)
        {
            _groundPositionHaneler.SetGround(_elements[element]);
            Debug.Log("choose " + (_elements[element]).ToString());
        }
    }
}
