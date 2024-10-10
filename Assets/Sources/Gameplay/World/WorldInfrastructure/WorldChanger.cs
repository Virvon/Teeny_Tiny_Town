using System;
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
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Services.PersistentProgress;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class WorldChanger : IBuildingGivable, ITileGetable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly World _world;
        private readonly IPersistentProgressService _persistentProgressService;

        private List<Tile> _tiles;

        public WorldChanger(IStaticDataService staticDataService, World world, IPersistentProgressService persistentProgressService)
        {
            _staticDataService = staticDataService;
            _world = world;

            _tiles = new();
            _persistentProgressService = persistentProgressService;
        }

        public event Action TilesChanged;

        public BuildingForPlacingInfo BuildingForPlacing { get; private set; }
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
            await changedTile.PutBuilding(GetBuilding(buildingType, gridPosition));

            AddNewBuilding();

            TilesChanged?.Invoke();
        }

        public async UniTask Update(ReadOnlyArray<TileInfrastructureData> tileDatas, BuildingForPlacingInfo buildingForPlacing)
        {
            foreach (TileInfrastructureData tileData in tileDatas)
            {
                Tile tile = GetTile(tileData.GridPosition);

                tile.Clean();
                await tile.PutBuilding(GetBuilding(tileData.BuildingType, tileData.GridPosition));
            }

            BuildingForPlacing = buildingForPlacing;

            TilesChanged?.Invoke();
        }

        public async UniTask ReplaceBuilding(Vector2Int fromBuildingGridPosition, BuildingType fromBuildingType, Vector2Int toBuildingGridPosition, BuildingType toBuildingType)
        {
            List<Tile> changedTiles = new();

            Tile fromTile = GetTile(fromBuildingGridPosition);
            await fromTile.PutBuilding(GetBuilding(toBuildingType, fromBuildingGridPosition));

            Tile toTile = GetTile(toBuildingGridPosition);
            await toTile.PutBuilding(GetBuilding(fromBuildingType, toBuildingGridPosition));

            AddNewBuilding();

            TilesChanged?.Invoke();
        }

        public Building GetBuilding(BuildingType type, Vector2Int gridPosition)
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
                    return new PayableBuilding(type, _staticDataService, _world.WorldData.WorldWallet, _persistentProgressService);
                case BuildingType.Stone:
                    return new Building(type);
                case BuildingType.Chest:
                    return new Chest(type, _staticDataService, gridPosition);
                case BuildingType.Lighthouse:
                    return new Lighthouse(type, _world.WorldData.WorldWallet, _persistentProgressService, this, gridPosition);
                case BuildingType.Crane:
                    return new Crane(type, this, gridPosition);
                default:
                    Debug.LogError("building not founded");
                    return null; ;
            }
        }

        public void RemoveBuilding(Vector2Int destroyBuildingGridPosition)
        {
            Tile tile = GetTile(destroyBuildingGridPosition);

            if (tile.Building.Type == BuildingType.Undefined)
                Debug.LogError("Can not destroy empty building");

            tile.RemoveBuilding();

            List<Tile> changedTiles = new() { tile };

            TilesChanged?.Invoke();
        }

        public Tile GetTile(Vector2Int gridPosition) =>
            _tiles.FirstOrDefault(tile => tile.GridPosition == gridPosition);

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

        public IEnumerable<int> GetLineNeighbors(int linePosition)
        {
            for (int i = linePosition - 1; i <= linePosition + 1; i++)
                yield return i;
        }

        private async UniTask Fill(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            List<RoadTile> roadTiles = new();
            List<TallTile> tallTiles = new();

            foreach (TileData tileData in _world.WorldData.Tiles)
            {
                Tile tile;

                switch (tileData.Type)
                {
                    case TileType.RoadGround:
                        RoadTile roadTile = new(tileData.Type, tileData.GridPosition, _staticDataService, GetBuilding(tileData.BuildingType, tileData.GridPosition), _world.WorldData, this);
                        tile = roadTile;
                        roadTiles.Add(roadTile);
                        break;
                    case TileType.TallGround:
                        TallTile tallTile = new(tileData.Type, tileData.GridPosition, _staticDataService, GetBuilding(tileData.BuildingType, tileData.GridPosition), _world.WorldData, this);
                        tile = tallTile;
                        tallTiles.Add(tallTile);
                        break;
                    case TileType.WaterSurface:
                        tile = new Tile(tileData.Type, tileData.GridPosition, _staticDataService, GetBuilding(tileData.BuildingType, tileData.GridPosition));
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

            foreach (Tile tile in _tiles)
                await tile.CreateRepresentation(tileRepresentationCreatable);
        }

        private void AddNewBuilding()
        {
            bool isPositionFree = false;

            while (isPositionFree == false)
            {
                Tile tile = _tiles[Random.Range(0, _tiles.Count)];

                if (tile.IsEmpty)
                {
                    List<BuildingType> availableBuildingTypes = _world.WorldData.AvailableBuildingForCreation;
                    BuildingType buildingType = availableBuildingTypes[Random.Range(0, availableBuildingTypes.Count)];

                    BuildingForPlacing = new BuildingForPlacingInfo(tile.GridPosition, buildingType);
                    isPositionFree = true;
                }
            }
        }
    }
}
