using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using UiCanvas = Assets.Sources.UI.UiCanvas;

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

            Container
                .BindFactory<string, UniTask<SelectFrame>, SelectFrame.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<SelectFrame>>();

            Container
                .BindFactory<string, UniTask<UiCanvas>, UiCanvas.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<UiCanvas>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, float, Transform, UniTask<Ground>, Ground.Factory>()
                .FromFactory<RefefencePrefabFactoryAsync<Ground>>();
        }
    }
}
