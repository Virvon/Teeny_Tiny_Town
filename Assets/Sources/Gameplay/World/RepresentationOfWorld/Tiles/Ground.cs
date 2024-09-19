using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Transform _buildingPoint;

        public Transform BuildingPoint => _buildingPoint;
    }
}
