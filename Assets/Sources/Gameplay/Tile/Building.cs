using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class Building : MonoBehaviour
    {
        public Ground Ground;

        public void ResetPosition()
        {
            transform.position = Ground.BuildingPoint.position;
        }
    }
}
