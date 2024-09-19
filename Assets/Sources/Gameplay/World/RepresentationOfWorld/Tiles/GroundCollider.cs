using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class GroundCollider : MonoBehaviour
    {
        public Tile Tile { get; private set; }

        private void Start() =>
            Tile = GetComponentInParent<Tile>();
    }
}
