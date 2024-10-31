using System.Collections.Generic;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Data.Sandbox;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Cysharp.Threading.Tasks;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using System;
using UnityEngine;
using Assets.Sources.Services.StaticDataService.Configs.World;
using System.Linq;

namespace Assets.Sources.Sandbox
{
    public class SandboxChanger : ICenterChangeable
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IStaticDataService _staticDataService;

        private List<SandboxTile> _tiles;

        public SandboxChanger(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
        }

        public event Action<Vector2Int, bool> CenterChanged;

        public async UniTask PutGround(Vector2Int gridPosition, SandboxGroundType type)
        {
            SandboxTile tile = GetTile(gridPosition);
            await tile.PutGround(type);
        }

        public async UniTask Generate(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            //TileRepresentationCreatable = tileRepresentationCreatable;

            await Fill(tileRepresentationCreatable);
            CenterChanged?.Invoke(_staticDataService.SandboxConfig.Size, false);
        }

        protected async UniTask Fill(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            _tiles = CreateTiles();

            InitializeAdjacentTiles(_tiles);

            foreach (SandboxTile tile in _tiles)
                await tile.CreateRepresentation(tileRepresentationCreatable);
        }

        private void InitializeAdjacentTiles(List<SandboxTile> tiles)
        {
            foreach (SandboxTile tile in tiles)
            {
                foreach (int positionX in GetLineNeighbors(tile.GridPosition.x))
                    TryAddNeighborTile(new Vector2Int(positionX, tile.GridPosition.y), tile);

                foreach (int positionY in GetLineNeighbors(tile.GridPosition.y))
                    TryAddNeighborTile(new Vector2Int(tile.GridPosition.x, positionY), tile);
            }
        }

        private void TryAddNeighborTile(Vector2Int gridPosition, SandboxTile tile)
        {
            SandboxTile adjacentTile = GetTile(gridPosition);

            if (adjacentTile != null && tile.GridPosition != adjacentTile.GridPosition)
            {
                tile.AddAdjacentTile(adjacentTile);
            }
        }

        public IEnumerable<int> GetLineNeighbors(int linePosition)
        {
            for (int i = linePosition - 1; i <= linePosition + 1; i++)
                yield return i;
        }

        private List<SandboxTile> CreateTiles()
        {
            List<SandboxTile> tiles = new();

            foreach (SandboxTileData tileData in _persistentProgressService.Progress.SandboxData.Tiles)
            {

                SandboxTile tile = GetGround(tileData);

                tiles.Add(tile);

                
            }

            return tiles;
        }

        private SandboxTile GetTile(Vector2Int gridPosition) =>
            _tiles.FirstOrDefault(tile => tile.GridPosition == gridPosition);

        private SandboxTile GetGround(SandboxTileData tileData)
        {
            return new SandboxTile(tileData, _staticDataService);

            //switch (tileData.Type)
            //{
            //    case TileType.RoadGround:
            //        RoadTile roadTile = new(
            //            tileData,
            //            tileType,
            //            StaticDataService,
            //            GetBuilding(tileData.BuildingType, tileData.GridPosition),
            //            WorldData,
            //            this);

            //        roadTiles.Add(roadTile);

            //        return roadTile;
            //    case TileType.TallGround:
            //        TallTile tallTile = new(
            //            tileData,
            //            tileType,
            //            StaticDataService,
            //            GetBuilding(tileData.BuildingType, tileData.GridPosition),
            //            WorldData,
            //            this);

            //        tallTiles.Add(tallTile);

            //        return tallTile;
            //    case TileType.WaterSurface:
            //        return new Tile(
            //            tileData,
            //            tileType,
            //            StaticDataService,
            //            GetBuilding(tileData.BuildingType, tileData.GridPosition));
            //    default:
            //        Debug.LogError("tile can not be null");
            //        return null;
            //}
        }
    }
}
