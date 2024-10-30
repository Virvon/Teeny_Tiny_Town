using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Data;
using Cysharp.Threading.Tasks;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public class WorldChanger : IWorldChanger, IBuildingsUpdatable
    {
        protected readonly IWorldData WorldData;
        protected readonly IStaticDataService StaticDataService;
        protected readonly NextBuildingForPlacingCreator NextBuildingForPlacingCreator;

        private List<Tile> _tiles;

        public WorldChanger(
            IStaticDataService staticDataService,
            IWorldData worldData,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            StaticDataService = staticDataService;
            WorldData = worldData;
            NextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
        }

        public event Action TilesChanged;
        public event Action UpdateFinished;
        public event Action<BuildingType> BuildingPlaced;
        public event Action<Vector2Int, bool> CenterChanged;

        public IReadOnlyList<Tile> Tiles => _tiles;

        protected ITileRepresentationCreatable TileRepresentationCreatable { get; private set; }

        public void Start()
        {
            TilesChanged?.Invoke();
            OnCenterChanged(false);
        }

        public async UniTask Generate(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            TileRepresentationCreatable = tileRepresentationCreatable;

            await Fill(tileRepresentationCreatable);
        }

        public async UniTask PlaceNewBuilding(Vector2Int gridPosition, BuildingType buildingType)
        {
            Tile changedTile = GetTile(gridPosition);
            await changedTile.PutBuilding(GetBuilding(buildingType, gridPosition));

            NextBuildingForPlacingCreator.MoveToNextBuilding(Tiles);
            TilesChanged?.Invoke();
            BuildingPlaced?.Invoke(buildingType);
        }

        public async UniTask Update()
        {
            TileData[] tileDatas = new TileData[WorldData.Tiles.Count];

            for(int i = 0; i < WorldData.Tiles.Count; i++)
                tileDatas[i] = new TileData(WorldData.Tiles[i].GridPosition, WorldData.Tiles[i].BuildingType);

            foreach (TileData tileData in tileDatas)
            {
                if (IsTileFitsIntoGrid(tileData.GridPosition) == false)
                    continue;

                Tile tile = GetTile(tileData.GridPosition);

                await tile.CleanAll();
            }
            
            tileDatas = tileDatas.Reverse().ToArray();

            foreach (TileData tileData in tileDatas)
            {
                if (IsTileFitsIntoGrid(tileData.GridPosition) == false)
                    continue;

                Tile tile = GetTile(tileData.GridPosition);

                await tile.UpdateBuilding(GetBuilding(tileData.BuildingType, tileData.GridPosition), this);
            }

            UpdateFinished?.Invoke();
            TilesChanged?.Invoke();
        }

        public async UniTask ReplaceBuilding(Vector2Int fromBuildingGridPosition, BuildingType fromBuildingType, Vector2Int toBuildingGridPosition, BuildingType toBuildingType)
        {
            List<UniTask> tasks = new();


            Tile fromTile = GetTile(fromBuildingGridPosition);
            tasks.Add(fromTile.PutBuilding(GetBuilding(toBuildingType, fromBuildingGridPosition)));

            Tile toTile = GetTile(toBuildingGridPosition);
            tasks.Add(toTile.PutBuilding(GetBuilding(fromBuildingType, toBuildingGridPosition)));

            await UniTask.WhenAll(tasks);

            NextBuildingForPlacingCreator.FindTileToPlacing(Tiles);

            TilesChanged?.Invoke();
        }

        public virtual Building GetBuilding(BuildingType type, Vector2Int gridPosition)
        {
            switch (type)
            {
                case BuildingType.Undefined:
                    return null;
                case BuildingType.Bush:
                    return new Building(type);
                case BuildingType.Tree:
                    return new Building(type);
                case BuildingType.House:
                    return new Building(type);
                case BuildingType.Stone:
                    return new Building(type);
                case BuildingType.Chest:
                    return new Chest(type, StaticDataService, gridPosition);
                case BuildingType.Lighthouse:
                    return new Building(type);
                case BuildingType.Crane:
                    return new Crane(type, this, gridPosition);
                default:
                    Debug.LogError("building not founded");
                    return null;
            }
        }

        public async UniTask RemoveBuilding(Vector2Int destroyBuildingGridPosition)
        {
            Tile tile = GetTile(destroyBuildingGridPosition);

            if (tile.Building.Type == BuildingType.Undefined)
                Debug.LogError("Can not destroy empty building");

            await tile.RemoveBuilding();

            TilesChanged?.Invoke();
        }

        public Tile GetTile(Vector2Int gridPosition) =>
            _tiles.FirstOrDefault(tile => tile.GridPosition == gridPosition);

        public IEnumerable<int> GetLineNeighbors(int linePosition)
        {
            for (int i = linePosition - 1; i <= linePosition + 1; i++)
                yield return i;
        }

        protected void Clean()
        {
            foreach (Tile tile in _tiles)
                tile.Destroy();

            _tiles.Clear();
        }

        protected async UniTask Fill(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            List<RoadTile> roadTiles = new();
            List<TallTile> tallTiles = new();

            _tiles = CreateTiles(roadTiles, tallTiles);

            InitializeAdjacentTiles(roadTiles);
            InitializeAdjacentTiles(tallTiles);
            InitializeAroundTiles(roadTiles);
            InitializeRoadTiles(roadTiles);

            foreach (Tile tile in _tiles)
                await tile.CreateRepresentation(tileRepresentationCreatable);
        }

        protected virtual List<Tile> CreateTiles(List<RoadTile> roadTiles, List<TallTile> tallTiles)
        {
            List<Tile> tiles = new();

            foreach (TileData tileData in WorldData.Tiles)
            {
                if (IsTileFitsIntoGrid(tileData.GridPosition) == false)
                    continue;

                Tile tile = GetGround(roadTiles, tallTiles, tileData);

                tiles.Add(tile);
            }

            return tiles;
        }

        protected bool IsTileFitsIntoGrid(Vector2Int gridPosition) =>
            gridPosition.x < WorldData.Size.x && gridPosition.y < WorldData.Size.y;

        protected Tile GetGround(List<RoadTile> roadTiles, List<TallTile> tallTiles, TileData tileData)
        {
            TileType tileType = StaticDataService.GetWorld<WorldConfig>(WorldData.Id).GetTileType(tileData.GridPosition);

            switch (tileType)
            {
                case TileType.RoadGround:
                    RoadTile roadTile = new(
                        tileData,
                        tileType,
                        StaticDataService,
                        GetBuilding(tileData.BuildingType, tileData.GridPosition),
                        WorldData,
                        this);

                    roadTiles.Add(roadTile);

                    return roadTile;
                case TileType.TallGround:
                    TallTile tallTile = new(
                        tileData,
                        tileType,
                        StaticDataService,
                        GetBuilding(tileData.BuildingType, tileData.GridPosition),
                        WorldData,
                        this);

                    tallTiles.Add(tallTile);

                    return tallTile;
                case TileType.WaterSurface:
                    return new Tile(
                        tileData,
                        tileType,
                        StaticDataService,
                        GetBuilding(tileData.BuildingType, tileData.GridPosition));
                default:
                    Debug.LogError("tile can not be null");
                    return null;
            }
        }

        protected void OnCenterChanged(bool isNeedAnimate) =>
            CenterChanged?.Invoke(WorldData.Size, isNeedAnimate);

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

            if (adjacentTile != null && tile.GridPosition != adjacentTile.GridPosition && tile.Type == adjacentTile.Type)
                tile.AddAdjacentTile(adjacentTile);
        }

        private void TryAddAroundTile(Vector2Int gridPosition, RoadTile tile)
        {
            RoadTile aroundTile = _tiles.FirstOrDefault(value => value.GridPosition == gridPosition) as RoadTile;

            if (aroundTile != null && tile != aroundTile)
                tile.AddAroundTile(aroundTile);
        }
    }
}
