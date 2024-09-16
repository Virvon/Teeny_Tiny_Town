using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Sources.Gameplay.Tile;
using Random = UnityEngine.Random;
using Assets.Sources.Services.StaticDataService;
using UnityEngine.InputSystem.Utilities;
using Assets.Sources.Gameplay.WorldGenerator.Comand;

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

        public event Action<List<Vector2Int>> TilesChanged;

        public IReadOnlyList<TileModel> Tiles => _tiles;
        public BuildingModel BuildingToPlacing { get; private set; }

        public void Generate()
        {
            Fill();
            InitializeAdjacentTiles();
            CreateBuildingToPlacing();
        }

        public void Work()
        {
            TilesChanged?.Invoke(GetGridPositions(_tiles));
        }

        public void Update(Vector2Int gridPosition, BuildingType buildingType)
        {
            bool chainCheakCompleted = false;
            TileModel updatedTile = GetTile(gridPosition);
            List<Vector2Int> changedTilePositions = new() { updatedTile.GridPosition };

            updatedTile.PutBuilding(buildingType);

            while (chainCheakCompleted == false)
            {
                List<TileModel> countedTiles = new();

                if(updatedTile.GetTilesChainCount(countedTiles) >= MinTilesCountToMerge)
                {
                    countedTiles.Remove(updatedTile);
                    changedTilePositions.AddRange(GetGridPositions(countedTiles));

                    foreach (TileModel tileModel in countedTiles)
                        tileModel.Clean();

                    updatedTile.UpdateBuilding();
                }
                else
                {
                    chainCheakCompleted = true;
                }
            }

            CreateBuildingToPlacing();

            TilesChanged?.Invoke(changedTilePositions);
        }

        public TileModel GetTile(Vector2Int gridPosition) =>
            _tiles.First(tile => tile.GridPosition == gridPosition);

        public void Update(ReadOnlyArray<TileData> tileDatas, BuildingModel buildingToPlacing)
        {
            foreach (TileData tileData in tileDatas)
            {
                GetTile(tileData.TileGridPosition).PutBuilding(tileData.BuildingType);
            }

            BuildingToPlacing = buildingToPlacing;

            TilesChanged?.Invoke(GetGridPositions(_tiles));
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
                    _tiles.Add(new TileModel(new Vector2Int(x, z), _staticDataService));
                }
            }
        }

        private void CreateBuildingToPlacing()
        {
            bool isPositionFree = false;

            while(isPositionFree == false)
            {
                TileModel tile = _tiles[Random.Range(0, _tiles.Count)];

                if(tile.BuildingType == BuildingType.Undefined)
                {
                    BuildingToPlacing = new BuildingModel(tile.GridPosition, BuildingType.Bush);
                    isPositionFree = true;
                }
            }
        }

        private List<Vector2Int> GetGridPositions(List<TileModel> tiles) =>
            tiles.Select(value => value.GridPosition).ToList();
    }
}
