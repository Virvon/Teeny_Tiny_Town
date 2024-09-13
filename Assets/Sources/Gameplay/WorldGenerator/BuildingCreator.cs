using Assets.Sources.Gameplay.Tile;
using UnityEngine;

namespace Assets.Sources.Gameplay.WorldGenerator
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
            Tile.Tile tile = GetRandomEmptyTile();

            Building building = Instantiate(_buildingPrefab, tile.Ground.transform.position, Quaternion.identity, tile.transform);
            building.Ground = tile.Ground;

            _buildingPositionHandler.Set(building);
        }

        private Tile.Tile GetRandomEmptyTile()
        {
            return _worldGenerator.Tiles[Random.Range(0, _worldGenerator.Tiles.Count - 1)];
        }

        private void OnBuildingCreated()
        {
            Create();
        }
    }
}
