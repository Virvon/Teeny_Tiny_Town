using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class GroundCollider : MonoBehaviour
    {
        public TileSelection TileSelection { get; private set; }

        private void Start()
        {
            TileSelection = GetComponentInParent<TileSelection>();
        }
    }
}
