using UnityEngine;

namespace Assets.Sources.WorldGenerator
{
    public class Building : MonoBehaviour
    {
        public Soil Soil;

        public void ResetPosition()
        {
            transform.position = Soil.BuildingPoint.position;
        }
    }
}
