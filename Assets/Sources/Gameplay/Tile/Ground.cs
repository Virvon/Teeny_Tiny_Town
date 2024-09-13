using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Transform _buildingPoint;

        public Transform BuildingPoint => _buildingPoint;
    }
}
