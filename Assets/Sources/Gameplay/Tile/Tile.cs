using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Ground _groundPrefab;
        [SerializeField] private Transform _soilPoint;

        public Ground Ground { get; private set; }

        private void Awake()
        {
            Ground = Instantiate(_groundPrefab, _soilPoint.position, Quaternion.identity, transform);
        }
    }
}
