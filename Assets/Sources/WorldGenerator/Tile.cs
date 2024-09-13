using UnityEngine;

namespace Assets.Sources.WorldGenerator
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Soil _soilPrefab;
        [SerializeField] private Transform _soilPoint;

        public Soil Soil { get; private set; }

        private void Awake()
        {
            Soil = Instantiate(_soilPrefab, _soilPoint.position, Quaternion.identity, transform);
        }
    }
}
