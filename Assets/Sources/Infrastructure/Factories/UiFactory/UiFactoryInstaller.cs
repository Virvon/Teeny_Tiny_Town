﻿using Assets.Sources.Gameplay.Store;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World.Panels.Reward;
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
                .BindFactory<AssetReferenceGameObject, Transform, UniTask<StoreItem>, StoreItem.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<StoreItem>>();

            Container
                .BindFactory<AssetReferenceGameObject, Transform, UniTask<RewardPanel>, RewardPanel.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<RewardPanel>>();
        }
    }
}
