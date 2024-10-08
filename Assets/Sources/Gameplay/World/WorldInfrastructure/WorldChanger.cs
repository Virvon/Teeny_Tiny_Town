﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Data;
using Cysharp.Threading.Tasks;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using UnityEngine.InputSystem.Utilities;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Services.StaticDataService.Configs.World;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class WorldChanger
    {
        private readonly IStaticDataService _staticDataService;
        private readonly World _world;

        private List<Tile> _tiles;

        public WorldChanger(IStaticDataService staticDataService, World world)
        {
            _staticDataService = staticDataService;
            _world = world;

            _tiles = new();
        }

        public event Action TilesChanged;

        public Building BuildingForPlacing { get; private set; }
        public IReadOnlyList<Tile> Tiles => _tiles;

        public event Action UpdatedInspect;

        public async UniTask Generate(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            await Fill(tileRepresentationCreatable);
            AddNewBuilding();

            TilesChanged?.Invoke();
        }

        public async UniTask PlaceNewBuilding(Vector2Int gridPosition, BuildingType buildingType)
        {
            UpdatedInspect?.Invoke();

            Tile changedTile = GetTile(gridPosition);
            await changedTile.PutBuilding(buildingType);

            AddNewBuilding();

            TilesChanged?.Invoke();
        }

        public async UniTask Update(ReadOnlyArray<TileInfrastructureData> tileDatas, Building buildingForPlacing)
        {
            foreach(TileInfrastructureData tileData in tileDatas)
            {
                Tile tile = GetTile(tileData.GridPosition);

                tile.Clean();
                await tile.PutBuilding(tileData.BuildingType);
            }

            BuildingForPlacing = buildingForPlacing;

            TilesChanged?.Invoke();
        }

        public async UniTask ReplaceBuilding(Vector2Int fromBuildingGridPosition, BuildingType fromBuildingType, Vector2Int toBuildingGridPosition, BuildingType toBuildingType)
        {
            List<Tile> changedTiles = new();

            Tile fromTile = GetTile(fromBuildingGridPosition);
            await fromTile.PutBuilding(toBuildingType);

            Tile toTile = GetTile(toBuildingGridPosition);
            await toTile.PutBuilding(fromBuildingType);

            AddNewBuilding();

            TilesChanged?.Invoke();
        }

        public void RemoveBuilding(Vector2Int destroyBuildingGridPosition)
        {
            Tile tile = GetTile(destroyBuildingGridPosition);

            if (tile.BuildingType == BuildingType.Undefined)
                Debug.LogError("can not destroy empty building");

            tile.RemoveBuilding();

            List<Tile> changedTiles = new() { tile };

            TilesChanged?.Invoke();
        }

        public Tile GetTile(Vector2Int gridPosition) =>
            _tiles.First(tile => tile.GridPosition == gridPosition);

        private void InitializeAroundTiles(List<RoadTile> roadTiles)
        {
            foreach (RoadTile tile in roadTiles)
            {
                foreach (int positionY in GetLineNeighbors(tile.GridPosition.y))
                {
                    foreach (int positionX in GetLineNeighbors(tile.GridPosition.x))
                        TryAddAroundTile(new Vector2Int(positionX, positionY), tile);
                }
            }
        }

        private void InitializeAdjacentTiles<TTile>(List<TTile> tiles)
            where TTile : TallTile
        {
            foreach (TTile tile in tiles)
            {
                foreach (int positionX in GetLineNeighbors(tile.GridPosition.x))
                    TryAddNeighborTile(new Vector2Int(positionX, tile.GridPosition.y), tile);

                foreach (int positionY in GetLineNeighbors(tile.GridPosition.y))
                    TryAddNeighborTile(new Vector2Int(tile.GridPosition.x, positionY), tile);
            }
        }

        private void InitializeRoadTiles(List<RoadTile> roadTiles)
        {
            foreach (RoadTile tile in roadTiles)
                tile.ValidateGroundType();

            foreach (RoadTile tile in roadTiles)
                tile.ValidateRoadType();
        }

        private void TryAddNeighborTile(Vector2Int gridPosition, TallTile tile)
        {
            TallTile adjacentTile = _tiles.FirstOrDefault(value => value.GridPosition == gridPosition) as TallTile;

            if (adjacentTile != null && tile != adjacentTile && tile.Type == adjacentTile.Type)
                tile.AddAdjacentTile(adjacentTile);
        }

        private void TryAddAroundTile(Vector2Int gridPosition, RoadTile tile)
        {
            RoadTile aroundTile = _tiles.FirstOrDefault(value => value.GridPosition == gridPosition) as RoadTile;

            if (aroundTile != null && tile != aroundTile)
                tile.AddAroundTile(aroundTile);
        }

        private IEnumerable<int> GetLineNeighbors(int linePosition)
        {
            for (int i = linePosition - 1; i <= linePosition + 1; i++)
                yield return i;
        }

        private async UniTask Fill(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            List<RoadTile> roadTiles = new();
            List<TallTile> tallTiles = new();

            foreach(TileData tileData in _world.WorldData.Tiles)
            {
                Tile tile;

                switch (tileData.Type)
                {
                    case TileType.RoadGround:
                        RoadTile roadTile = new (tileData.Type, tileData.GridPosition,  _staticDataService, tileData.BuildingType);
                        tile = roadTile;
                        roadTiles.Add(roadTile);
                        break;
                    case TileType.TallGround:
                        TallTile tallTile = new (tileData.Type, tileData.GridPosition, _staticDataService, tileData.BuildingType);
                        tile = tallTile;
                        tallTiles.Add(tallTile);
                        break;
                    case TileType.WaterSurface:
                        tile = new Tile(tileData.Type, tileData.GridPosition, _staticDataService, tileData.BuildingType);
                        break;
                    default:
                        tile = null;
                        Debug.LogError("tile can not be null");
                        break;
                }
                             
                _tiles.Add(tile);
            }

            InitializeAdjacentTiles(roadTiles);
            InitializeAdjacentTiles(tallTiles);
            InitializeAroundTiles(roadTiles);
            InitializeRoadTiles(roadTiles);

            foreach(Tile tile in _tiles)
                await tile.CreateRepresentation(tileRepresentationCreatable);
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
