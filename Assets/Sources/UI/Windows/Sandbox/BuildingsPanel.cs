using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class BuildingsPanel : SandboxPanel
    {
        [Inject]
        private async void Construct(IUiFactory uiFactory, IPersistentProgressService persistentProgressService)
        {
            foreach (Data.BuildingData buildingData in persistentProgressService.Progress.BuildingDatas)
                await uiFactory.CreateSandboxPanelElement(buildingData.Type, Content);
        }
    }
}
