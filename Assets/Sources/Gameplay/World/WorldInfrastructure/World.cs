using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Assets.Sources.Services.StaticDataService;
using UnityEngine.InputSystem.Utilities;
using Assets.Sources.Gameplay.GameplayMover;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class World
    {
        private const uint _length = 5;
        private const uint _width = 5;
        private const uint MinTilesCountToMerge = 3;

        private readonly IStaticDataService _staticDataService;

        private List<Tile> _tiles;

        public World(IStaticDataService staticDataService)
        {
            _tiles = new();
            _staticDataService = staticDataService;
        }

        public event Action<List<Vector2Int>> TilesChanged;

        public IReadOnlyList<Tile> Tiles => _tiles;
        public Building BuildingForPlacing { get; private set; }

        public void Generate()
        {
            Fill();
            InitializeAdjacentTiles();
            AddNewBuilding();
        }

        public void Work() =>
            TilesChanged?.Invoke(GetGridPositions(_tiles));

        public void Update(Vector2Int gridPosition, BuildingType buildingType)
        {
            List<Vector2Int> changedTilePositions = CheckChangedTilePositions(gridPosition, buildingType);

            AddNewBuilding();

            TilesChanged?.Invoke(changedTilePositions);
        }

        public void ReplaceBuilding(Vector2Int fromBuildingGridPosition, BuildingType fromBuildingType, Vector2Int toBuildingGridPosition, BuildingType toBuildingType)
        {
            List<Vector2Int> changedTilePositions = new();

            changedTilePositions.AddRange(CheckChangedTilePositions(toBuildingGridPosition, toBuildingType));
            changedTilePositions.AddRange(CheckChangedTilePositions(fromBuildingGridPosition, fromBuildingType));

            AddNewBuilding();

            TilesChanged?.Invoke(changedTilePositions);
        }

        public void RemoveBuilding(Vector2Int destroyBuildingGridPosition)
        {
            Tile tile = GetTile(destroyBuildingGridPosition);

            if (tile.BuildingType == BuildingType.Undefined)
                Debug.LogError("can not destroy empty building");

            tile.Clean();
            TilesChanged?.Invoke(new List<Vector2Int>() { destroyBuildingGridPosition });
        }

        public Tile GetTile(Vector2Int gridPosition) =>
            _tiles.First(tile => tile.GridPosition == gridPosition);

        public void Update(ReadOnlyArray<TileData> tileDatas, Building buildingToPlacing)
        {
            foreach (TileData tileData in tileDatas)
                GetTile(tileData.GridPosition).PutBuilding(tileData.BuildingType);

            BuildingForPlacing = buildingToPlacing;

            TilesChanged?.Invoke(GetGridPositions(_tiles));
        }
        private List<Vector2Int> CheckChangedTilePositions(Vector2Int firstTileGridPosition, BuildingType firstTileBuildingType)
        {
            Tile updatedTile = GetTile(firstTileGridPosition);
            bool chainCheakCompleted = false;
            List<Vector2Int> changedTilePositions = new() { updatedTile.GridPosition };

            updatedTile.PutBuilding(firstTileBuildingType);

            if (updatedTile.BuildingType == BuildingType.Undefined)
                return changedTilePositions;

            while (chainCheakCompleted == false)
            {
                List<Tile> countedTiles = new();

                if (updatedTile.GetTilesChainCount(countedTiles) >= MinTilesCountToMerge)
                {
                    Debug.Log(countedTiles.Count);

                    countedTiles.Remove(updatedTile);
                    changedTilePositions.AddRange(GetGridPositions(countedTiles));

                    foreach (Tile tileModel in countedTiles)
                        tileModel.Clean();

                    updatedTile.UpdateBuilding();
                }
                else
                {
                    chainCheakCompleted = true;
                }
            }

            return changedTilePositions;
        }


        private void InitializeAdjacentTiles()
        {
            foreach (Tile tile in _tiles)
            {
                foreach (int positionX in GetLineNeighbors(tile.GridPosition.x))
                    TryAddNeighborTile(new Vector2Int(positionX, tile.GridPosition.y), tile);

                foreach (int positionY in GetLineNeighbors(tile.GridPosition.y))
                    TryAddNeighborTile(new Vector2Int(tile.GridPosition.x, positionY), tile);
            }
        }

        private void TryAddNeighborTile(Vector2Int gridPosition, Tile tile)
        {
            Tile adjacentTile = _tiles.FirstOrDefault(value => value.GridPosition == gridPosition);

            if (adjacentTile != null && tile != adjacentTile)
                tile.AddAdjacentTile(adjacentTile);
        }

        private IEnumerable<int> GetLineNeighbors(int linePosition)
        {
            for (int i = linePosition - 1; i <= linePosition + 1; i++)
                yield return i;
        }

        private void Fill()
        {
            for (int x = 0; x < _length; x++)
            {
                for (int z = 0; z < _width; z++)
                    _tiles.Add(new Tile(new Vector2Int(x, z), _staticDataService));
            }
        }

        private void AddNewBuilding()
        {
            bool isPositionFree = false;

            while (isPositionFree == false)
            {
                Tile tile = _tiles[Random.Range(0, _tiles.Count)];

                if (tile.BuildingType == BuildingType.Undefined)
                {
                    BuildingForPlacing = new Building(tile.GridPosition, BuildingType.Bush);
                    isPositionFree = true;
                }
            }
        }

        private List<Vector2Int> GetGridPositions(List<Tile> tiles) =>
            tiles.Select(value => value.GridPosition).ToList();
    }
}
