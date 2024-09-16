using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Services.StaticDataService;

namespace Assets.Sources.Gameplay.WorldGenerator.World
{
    public class World
    {
        private const uint _length = 5;
        private const uint _width = 5;
        private const uint MinTilesCountToMerge = 3;

        private readonly IStaticDataService _staticDataService;

        private List<TileModel> _tiles;

        public World(IStaticDataService staticDataService)
        {
            _tiles = new();
            _staticDataService = staticDataService;
        }

        public event Action<Vector2Int> TileCleaned;
        public event Action<Vector2Int, BuildingType> TileBuildingUpdated;

        public IReadOnlyList<TileModel> Tiles => _tiles;

        public void Generate()
        {
            Fill();
            InitializeAdjacentTiles();
        }

        public void Update(Vector2Int gridPosition, BuildingType buildingType)
        {
            TileModel tile = GetTile(gridPosition);

            tile.ChangeBuilding(buildingType);

            List<TileModel> countedTiles = new ();

            if(tile.GetTilesChainCount(countedTiles) >= MinTilesCountToMerge)
            {
                BuildingType nextBuildingType = _staticDataService.GetMerge(tile.BuildingType).NextBuilding;

                Clean(countedTiles);
                UpgradeBuilding(tile, nextBuildingType);
            }
        }

        private void UpgradeBuilding(TileModel tile, BuildingType buildingType)
        {
            tile.ChangeBuilding(buildingType);
            TileBuildingUpdated?.Invoke(tile.GridPosition, buildingType);
        }

        private void Clean(List<TileModel> tiles)
        {
            foreach(TileModel tile in tiles)
            {
                tile.ChangeBuilding(BuildingType.Undefined);
                TileCleaned?.Invoke(tile.GridPosition);
            }
        }

        private void InitializeAdjacentTiles()
        {
            foreach(TileModel tile in _tiles)
            {
                foreach(int positionX in GetLineNeighbors(tile.GridPosition.x))
                    TryAddNeighborTile(new Vector2Int(positionX, tile.GridPosition.y), tile);

                foreach (int positionY in GetLineNeighbors(tile.GridPosition.y))
                    TryAddNeighborTile(new Vector2Int(tile.GridPosition.x, positionY), tile);
            }
        }

        private void TryAddNeighborTile(Vector2Int gridPosition, TileModel tile)
        {
            TileModel adjacentTile = _tiles.FirstOrDefault(value => value.GridPosition == gridPosition);

            if (adjacentTile != null && tile != adjacentTile)
                tile.AddAdjacentTile(adjacentTile);
        }

        private IEnumerable<int> GetLineNeighbors(int linePosition)
        {
            for(int i = linePosition - 1; i <= linePosition + 1; i++)
                yield return i;
        }

        private void Fill()
        {
            for (int x = 0; x < _length; x++)
            {
                for (int z = 0; z < _width; z++)
                {
                    _tiles.Add(new TileModel(new Vector2Int(x, z)));
                }
            }
        }

        private TileModel GetTile(Vector2Int gridPosition) => 
            _tiles.First(tile => tile.GridPosition == gridPosition);
    }
}
