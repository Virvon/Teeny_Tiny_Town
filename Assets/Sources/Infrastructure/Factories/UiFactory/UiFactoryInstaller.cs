using Assets.Sources.Services.AssetManagement;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactoryInstaller : Installer<UiFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IUiFactory>()
                .To<UiFactory>()
                .AsSingle();

            Container
                .BindFactory<AssetReferenceGameObject, UniTask<Window>, Window.Factory>()
                .FromFactory<RefefencePrefabFactoryAsync<Window>>();
        }
    }
}
