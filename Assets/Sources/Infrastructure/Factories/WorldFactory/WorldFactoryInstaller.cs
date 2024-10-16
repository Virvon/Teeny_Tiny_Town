﻿using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.WorldFactory
{
    public class WorldFactoryInstaller : Installer<WorldFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IWorldFactory>()
                .To<WorldFactory>()
                .AsSingle();

            Container
                .BindFactory<string, UniTask<WorldGenerator>, WorldGenerator.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<WorldGenerator>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<BuildingRepresentation>, BuildingRepresentation.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<BuildingRepresentation>>();

            Container
                .BindFactory<string, Vector3, Transform, UniTask<TileRepresentation>, TileRepresentation.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<TileRepresentation>>();

            Container
                .BindFactory<string, UniTask<SelectFrame>, SelectFrame.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<SelectFrame>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, float, Transform, UniTask<Ground>, Ground.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<Ground>>();

            Container
                .BindFactory<string, UniTask<BuildingMarker>, BuildingMarker.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<BuildingMarker>>();

            Container
                .BindFactory<string, UniTask<ActionHandlerSwitcher>, ActionHandlerSwitcher.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<ActionHandlerSwitcher>>();
        }
    }
}
