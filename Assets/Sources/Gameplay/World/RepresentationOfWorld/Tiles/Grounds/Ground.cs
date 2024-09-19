using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Transform _buildingPoint;

        public Transform BuildingPoint => _buildingPoint;

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, float, Transform, UniTask<Ground>>
        {
        }
    }
}
