﻿using System;
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
        private const uint MinTilesCountToBuildTrail = 3;

        private readonly IStaticDataService _staticDataService;

        private List<Tile> _tiles;

        public World(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;

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

            changedTiles = changedTiles.Union(GetAllUpdatedByGroundTiles(changedTiles)).ToList();

            TilesChanged?.Invoke(changedTiles);
        }

        public Tile GetTile(Vector2Int gridPosition) =>
            _tiles.First(tile => tile.GridPosition == gridPosition);

        public void Update(ReadOnlyArray<TileData> tileDatas, Building buildingToPlacing)
        {
            foreach (TileData tileData in tileDatas)
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
            changedTiles = changedTiles.Union(GetAllUpdatedByGroundTiles(changedTiles)).ToList();

            return changedTiles;
        }

        private static List<Tile> GetAllUpdatedByGroundTiles(List<Tile> changedTiles)
        {
            List<Tile> countedTiles = new();
            List<Tile> changedByGroundTiles = new();

            foreach (Tile tile in changedTiles)
                tile.ChangeGroudsInChain(countedTiles, changedByGroundTiles);

            return changedByGroundTiles;
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
            }
        }

        private void TryAddNeighborTile(Vector2Int gridPosition, Tile tile)
        {
            Tile adjacentTile = _tiles.FirstOrDefault(value => value.GridPosition == gridPosition);

            if (adjacentTile != null && tile != adjacentTile)
                tile.Init(adjacentTile);
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
    }
}