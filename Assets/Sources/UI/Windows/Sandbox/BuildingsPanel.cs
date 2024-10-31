using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Sandbox.ActionHandler;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class BuildingsPanel : SandboxPanel
    {
        private BuildingPositionHandler _buildingPositionHandler;

        private Dictionary<SandboxPanelElement, BuildingType> _elements;

        [Inject]
        private async void Construct(IUiFactory uiFactory, IPersistentProgressService persistentProgressService, BuildingPositionHandler buildingPositionHandler)
        {
            _buildingPositionHandler = buildingPositionHandler;

            _elements = new();

            foreach (Data.BuildingData buildingData in persistentProgressService.Progress.BuildingDatas)
            {
                SandboxPanelElement sandboxPanelElement = await uiFactory.CreateSandboxPanelElement(buildingData.Type, Content);
                _elements.Add(sandboxPanelElement, buildingData.Type);

                sandboxPanelElement.Clicked += OnElementClicked;
            }

            _buildingPositionHandler.SetBuilding(_elements.Values.First());
        }

        private void OnDestroy()
        {
            foreach (SandboxPanelElement sandboxPanelElement in _elements.Keys)
                sandboxPanelElement.Clicked -= OnElementClicked;
        }

        private void OnElementClicked(SandboxPanelElement element)
        {
            _buildingPositionHandler.SetBuilding(_elements[element]);
        }
    }
}
