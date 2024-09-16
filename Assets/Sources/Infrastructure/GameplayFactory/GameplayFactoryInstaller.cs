﻿using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Gameplay.WorldGenerator;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.GameplayFactory
{
    public class GameplayFactoryInstaller : Installer<GameplayFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameplayFactory>()
                .To<GameplayFactory>()
                .AsSingle();

            Container
                .BindFactory<string, UniTask<WorldGenerator>, WorldGenerator.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<WorldGenerator>>();

            Container
                .BindFactory<string, Vector3, Transform, UniTask<Tile>, Tile.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Tile>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<Building>, Building.Factory>()
                .FromFactory<RefefencePrefabFactoryAsync<Building>>();
        }
    }
}
