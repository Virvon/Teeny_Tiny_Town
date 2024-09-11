using UnityEngine;

namespace Assets.Sources.WorldGenerator
{
    public class Soil : MonoBehaviour
    {
        [SerializeField] private Transform _buildingPoint;

        public Transform BuildingPoint => _buildingPoint;
    }
}
