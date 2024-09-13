using UnityEngine;

namespace Assets.Sources.WorldGenerator
{
    public class BuildingCreator : MonoBehaviour
    {
        [SerializeField] private Building _buildingPrefab;
        [SerializeField] private WorldGenerator _worldGenerator;
        [SerializeField] private BuildingPositionHandler _buildingPositionHandler;

        private void Start()
        {
            Create();

            _buildingPositionHandler.BuildingCreated += OnBuildingCreated;
        }

        private void Create()
        {
            Tile tile = GetRandomEmptyTile();

            Building building = Instantiate(_buildingPrefab, tile.Soil.transform.position, Quaternion.identity, tile.transform);
            building.Soil = tile.Soil;

            _buildingPositionHandler.Set(building);
        }

        private Tile GetRandomEmptyTile()
        {
            return _worldGenerator.Tiles[Random.Range(0, _worldGenerator.Tiles.Count - 1)];
        }

        private void OnBuildingCreated()
        {
            Create();
        }
    }
}
