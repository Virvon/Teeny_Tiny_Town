using UnityEngine;

namespace Assets.Sources.WorldGenerator
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Soil _soil;
        [SerializeField] private Transform _soilPoint;

        public Soil Soil;

        private void Awake()
        {
            Soil = Instantiate(_soil, _soilPoint.position, Quaternion.identity, transform);
        }
    }
}
