using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public interface IUiFactory
    {
        UniTask<Window> CreateWindow(WindowType type);
    }
}