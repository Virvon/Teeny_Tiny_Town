using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class GroundCollider : MonoBehaviour
    {
        public Tile Tile { get; private set; }

        private void Start()
        {
            Tile = GetComponentInParent<Tile>();
        }
    }
}
