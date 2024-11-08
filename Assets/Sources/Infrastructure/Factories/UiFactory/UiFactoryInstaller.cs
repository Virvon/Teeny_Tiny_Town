using Assets.Sources.Services.AssetManagement;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.MapSelection;
using Assets.Sources.UI.Windows.Sandbox;
using Assets.Sources.UI.Windows.World.Panels;
using Assets.Sources.UI.Windows.World.Panels.AdditionalBonusOffer;
using Assets.Sources.UI.Windows.World.Panels.Reward;
using Assets.Sources.UI.Windows.World.Panels.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
                .FromFactory<ReferencePrefabFactoryAsync<Window>>();

            Container
                .BindFactory<AssetReferenceGameObject, Transform, UniTask<BuildingStoreItem>, BuildingStoreItem.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<BuildingStoreItem>>();

            Container
                .BindFactory<AssetReferenceGameObject, Transform, UniTask<RewardPanel>, RewardPanel.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<RewardPanel>>();

            Container
                .BindFactory<AssetReferenceGameObject, Transform, UniTask<QuestPanel>, QuestPanel.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<QuestPanel>>();

            Container
                .BindFactory<string, Transform, UniTask<RemainingMovesPanel>, RemainingMovesPanel.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<RemainingMovesPanel>>();

            Container
                .BindFactory<AssetReferenceGameObject, Transform, UniTask<AdditionalBonusOfferItem>, AdditionalBonusOfferItem.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<AdditionalBonusOfferItem>>();

            Container
                .BindFactory<AssetReferenceGameObject, Transform, UniTask<GainStoreItemPanel>, GainStoreItemPanel.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<GainStoreItemPanel>>();

            Container
                .BindFactory<string, Transform, UniTask<SandboxPanelElement>, SandboxPanelElement.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<SandboxPanelElement>>();

            Container
                .BindFactory<string, Transform, UniTask<RotationPanel>, RotationPanel.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<RotationPanel>>();

            Container
                .BindFactory<string, UniTask<Blur>, Blur.BlurFactory>()
                .FromFactory<KeyPrefabFactoryAsync<Blur>>();

            Container
                .BindFactory<string, Transform, UniTask<PeculiarityIconPanel>, PeculiarityIconPanel.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<PeculiarityIconPanel>>();

            Container
                .BindFactory<string, Transform, UniTask<LockIcon>, LockIcon.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<LockIcon>>();
        }
    }
}
