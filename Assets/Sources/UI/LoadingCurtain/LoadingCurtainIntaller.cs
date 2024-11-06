using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.UI.LoadingCurtain
{
    public class LoadingCurtainIntaller : Installer<LoadingCurtainIntaller>
    {
        public override void InstallBindings()
        {
            Container
                .BindFactory<string, UniTask<LoadingCurtain>, LoadingCurtain.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<LoadingCurtain>>();

            Container
                .BindInterfacesAndSelfTo<LoadingCurtainProxy>()
                .AsSingle();
        }
    }
}