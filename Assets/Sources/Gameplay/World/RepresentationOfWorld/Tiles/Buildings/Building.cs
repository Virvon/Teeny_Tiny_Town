using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings
{
    public class Building : MonoBehaviour
    {
        public BuildingType Type { get; private set; }

        public virtual void Init(BuildingType type) =>
            Type = type;

        public void Destroy() =>
            Destroy(gameObject);

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<Building>>
        {
        }
    }
}
