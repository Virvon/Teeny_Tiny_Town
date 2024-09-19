using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class Building : MonoBehaviour
    {
        public BuildingType Type { get; private set; }

        public void Init(BuildingType type) =>
            Type = type;

        public void Destroy() =>
            Destroy(gameObject);

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<Building>>
        {
        }
    }
}
