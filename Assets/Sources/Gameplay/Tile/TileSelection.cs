using Assets.Sources.Gameplay.WorldGenerator;
using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class TileSelection : MonoBehaviour
    {
        [SerializeField] private GroundCreator _groundCreator;
        [SerializeField] private TileMerge _tileMerge;

        public void Select(SelectFrame selectFrame, Vector3 selectFramePositionOffset)
        {
            selectFrame.transform.position = _groundCreator.Ground.BuildingPoint.position + selectFramePositionOffset;
        }

        public void PutBuilding(Building building)
        {
            building.transform.position = _groundCreator.Ground.BuildingPoint.position;
        }

        public void SetBuilding(Building building)
        {
            _tileMerge.SetBuilding(building);
        }
    }
}
