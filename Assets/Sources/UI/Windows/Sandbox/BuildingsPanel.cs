using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Sandbox.ActionHandler;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
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
        private async void Construct(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            BuildingPositionHandler buildingPositionHandler,
            IStaticDataService staticDataService)
        {
            _buildingPositionHandler = buildingPositionHandler;

            _elements = new();

            foreach (Data.BuildingData buildingData in persistentProgressService.Progress.BuildingDatas)
            {
                BuildingConfig config = staticDataService.GetBuilding<BuildingConfig>(buildingData.Type);

                if (buildingData.IsUnlocked)
                {
                    SandboxPanelElement sandboxPanelElement = await uiFactory.CreateSandboxPanelElement(Content, config.IconAssetReference);
                    _elements.Add(sandboxPanelElement, buildingData.Type);

                    sandboxPanelElement.Clicked += OnElementClicked;
                }
                else
                {
                    SandboxPanelElement sandboxPanelElement = await uiFactory.CreateSandboxPanelElement(Content, config.LockIconAssetReference);
                    await uiFactory.CreateLockIcon(sandboxPanelElement.transform);
                }
            }

            if (_elements.Count > 0)
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
