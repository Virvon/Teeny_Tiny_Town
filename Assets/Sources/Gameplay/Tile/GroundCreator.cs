using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class GroundCreator : MonoBehaviour
    {
        [SerializeField] private Ground _groundPrefab;
        [SerializeField] private Transform _groundPoint;

        public Ground Ground { get; private set; }

        private void Awake()
        {
            Ground = Instantiate(_groundPrefab, _groundPoint.position, Quaternion.identity, transform);
        }
    }
}
