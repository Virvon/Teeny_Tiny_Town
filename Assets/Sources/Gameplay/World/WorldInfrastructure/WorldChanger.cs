using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Assets.Sources.Services.StaticDataService;
using UnityEngine.InputSystem.Utilities;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Data;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class WorldChanger
    {
        private const uint MinTilesCountToMerge = 3;

        private readonly IStaticDataService _staticDataService;
        private World _world;
        private List<Tile> _tiles;

        public WorldChanger(IStaticDataService staticDataService, World world)
        {
            _staticDataService = staticDataService;
            _world = world;

            _tiles = new();
        }

        public event Action<List<Tile>> TilesChanged;

        public IReadOnlyList<Tile> Tiles => _tiles;
        public Building BuildingForPlacing { get; private set; }

        public void Generate()
        {
            Fill();
            InitializeAdjacentTiles();
            AddNewBuilding();
        }

        public void Work() =>
            TilesChanged?.Invoke(_tiles);

        public void PlaceNewBuilding(Vector2Int gridPosition, BuildingType buildingType)
        {
            List<Tile> changedTiles = CheckChangedTiles(gridPosition, buildingType);

            AddNewBuilding();

            TilesChanged?.Invoke(changedTiles);
        }

        public void ReplaceBuilding(Vector2Int fromBuildingGridPosition, BuildingType fromBuildingType, Vector2Int toBuildingGridPosition, BuildingType toBuildingType)
        {
            List<Tile> changedTiles = new();

            changedTiles = changedTiles.Union(CheckChangedTiles(fromBuildingGridPosition, toBuildingType)).ToList();
            changedTiles = changedTiles.Union(CheckChangedTiles(toBuildingGridPosition, fromBuildingType)).ToList();

            AddNewBuilding();

            TilesChanged?.Invoke(changedTiles);
        }

        public void RemoveBuilding(Vector2Int destroyBuildingGridPosition)
        {
            Tile tile = GetTile(destroyBuildingGridPosition);

            if (tile.BuildingType == BuildingType.Undefined)
                Debug.LogError("can not destroy empty building");

            tile.RemoveBuilding();

            List<Tile> changedTiles = new() { tile };

            changedTiles = changedTiles.Union(GetAllChangedByGroundTiles(changedTiles)).ToList();
            changedTiles = changedTiles.Union(GetAllUpdatedByRoadTiles(changedTiles)).ToList();

            TilesChanged?.Invoke(changedTiles);
        }

        public Tile GetTile(Vector2Int gridPosition) =>
            _tiles.First(tile => tile.GridPosition == gridPosition);

        public void Update(ReadOnlyArray<TileInfrastructureData> tileDatas, Building buildingToPlacing)
        {
            foreach (TileInfrastructureData tileData in tileDatas)
            {
                Tile tile = GetTile(tileData.GridPosition);

                tile.PutBuilding(tileData.BuildingType);
                tile.PutGround(tileData.GroundType, tileData.GroundRotation);
            }

            BuildingForPlacing = buildingToPlacing;

            TilesChanged?.Invoke(_tiles);
        }

        private List<Tile> CheckChangedTiles(Vector2Int firstTileGridPosition, BuildingType placedBuildingType)
        {
            Tile changedTile = GetTile(firstTileGridPosition);
            changedTile.PutBuilding(placedBuildingType);

            List<Tile> changedTiles = new () { changedTile };

            changedTiles = changedTiles.Union(GetAllUpdatedByBuildingTiles(changedTile)).ToList();
            changedTiles = changedTiles.Union(GetAllChangedByGroundTiles(changedTiles)).ToList();
            changedTiles = changedTiles.Union(GetAllUpdatedByRoadTiles(changedTiles)).ToList();

            return changedTiles;
        }

        private List<Tile> GetAllChangedByGroundTiles(List<Tile> changedByBuildingTiles)
        {
            List<Tile> countedTiles = new();
            List<Tile> changedTiles = new();

            foreach(Tile tile in changedByBuildingTiles)
                tile.ChangeGroundsInChain(countedTiles, changedTiles);

            return changedTiles;
        }

        private List<Tile> GetAllUpdatedByRoadTiles(List<Tile> changedByGroundTiles)
        {
            List<Tile> countedTiles = new();
            List<Tile> changedTiles = new();

            foreach (Tile tile in changedByGroundTiles)
                tile.ChangeRoadsInChain(countedTiles, changedTiles);

            return changedTiles;
        }

        private List<Tile> GetAllUpdatedByBuildingTiles(Tile changedTile)
        {
            List<Tile> changedTiles = new() { changedTile };

            if (changedTile.IsEmpty)
                return changedTiles;

            bool chainCheakCompleted = false;

            while (chainCheakCompleted == false)
            {
                List<Tile> countedTiles = new();

                if (changedTile.GetBuildingsChainLength(countedTiles) >= MinTilesCountToMerge)
                {
                    changedTiles.Union(countedTiles);

                    List<Tile> tilesForRemoveBuildings = countedTiles;
                    tilesForRemoveBuildings.Remove(changedTile);

                    foreach (Tile tile in countedTiles)
                        tile.RemoveBuilding();

                    changedTile.UpdateBuilding();
                }
                else
                {
                    chainCheakCompleted = true;
                }
            }

            return changedTiles;
        }

        private void InitializeAdjacentTiles()
        {
            foreach (Tile tile in _tiles)
            {
                foreach (int positionX in GetLineNeighbors(tile.GridPosition.x))
                    TryAddNeighborTile(new Vector2Int(positionX, tile.GridPosition.y), tile);

                foreach (int positionY in GetLineNeighbors(tile.GridPosition.y))
                    TryAddNeighborTile(new Vector2Int(tile.GridPosition.x, positionY), tile);

                foreach(int positionY in GetLineNeighbors(tile.GridPosition.y))
                {
                    foreach (int positionX in GetLineNeighbors(tile.GridPosition.x))
                        TryAddAroundTile(new Vector2Int(positionX, positionY), tile);
                }
            }
        }

        private void TryAddNeighborTile(Vector2Int gridPosition, Tile tile)
        {
            Tile adjacentTile = _tiles.FirstOrDefault(value => value.GridPosition == gridPosition);

            if (adjacentTile != null && tile != adjacentTile)
                tile.AddAdjacentTile(adjacentTile);
        }

        private void TryAddAroundTile(Vector2Int gridPosition, Tile tile)
        {
            Tile aroundTile = _tiles.FirstOrDefault(value => value.GridPosition == gridPosition);

            if (aroundTile != null && tile != aroundTile)
                tile.AddAroundTile(aroundTile);
        }

        private IEnumerable<int> GetLineNeighbors(int linePosition)
        {
            for (int i = linePosition - 1; i <= linePosition + 1; i++)
                yield return i;
        }

        private void Fill()
        {
            foreach(TileData tileData in _world.WorldData.Tiles)
                _tiles.Add(new Tile(tileData.GridPosition, _staticDataService, tileData.BuildingType));
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
    }
}
