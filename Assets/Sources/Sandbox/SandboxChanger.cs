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

        public async UniTask Generate(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            //TileRepresentationCreatable = tileRepresentationCreatable;

            await Fill(tileRepresentationCreatable);
            CenterChanged?.Invoke(_staticDataService.SandboxConfig.Size, false);
        }

        protected async UniTask Fill(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            List<RoadTile> roadTiles = new();
            List<TallTile> tallTiles = new();

            _tiles = CreateTiles(roadTiles, tallTiles);

            //InitializeAdjacentTiles(roadTiles);
            //InitializeAdjacentTiles(tallTiles);
            //InitializeAroundTiles(roadTiles);
            //InitializeRoadTiles(roadTiles);

            foreach (SandboxTile tile in _tiles)
                await tile.CreateRepresentation(tileRepresentationCreatable);
        }

        private List<SandboxTile> CreateTiles(List<RoadTile> roadTiles, List<TallTile> tallTiles)
        {
            List<SandboxTile> tiles = new();

            foreach (SandboxTileData tileData in _persistentProgressService.Progress.SandboxData.Tiles)
            {

                SandboxTile tile = GetGround(roadTiles, tallTiles, tileData);

                tiles.Add(tile);

                
            }

            return tiles;
        }

        private SandboxTile GetGround(List<RoadTile> roadTiles, List<TallTile> tallTiles, SandboxTileData tileData)
        {
            return new SandboxTile(tileData.Type, tileData);

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
