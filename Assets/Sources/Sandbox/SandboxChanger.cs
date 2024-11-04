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
using System.Threading.Tasks;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;

namespace Assets.Sources.Sandbox
{
    public class SandboxChanger : ICenterChangeable
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IStaticDataService _staticDataService;

        private List<SandboxTile> _tiles;
        private bool _isTileChangedComplete;

        public SandboxChanger(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;

            _isTileChangedComplete = true;
        }

        public event Action<Vector2Int, bool> CenterChanged;

        public async UniTask PutGround(Vector2Int gridPosition, SandboxGroundType type)
        {
            if (_isTileChangedComplete == false)
                return;

            _isTileChangedComplete = false;

            SandboxTile tile = GetTile(gridPosition);
            await tile.PutGround(type);

            _isTileChangedComplete = true;
        }

        public async UniTask ClearTile(Vector2Int gridPosition)
        {
            if (_isTileChangedComplete == false)
                return;

            _isTileChangedComplete = false;

            await GetTile(gridPosition).CleanAll();

            _isTileChangedComplete = true;
        }

        public async UniTask Generate(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            await Fill(tileRepresentationCreatable);
            CenterChanged?.Invoke(_staticDataService.SandboxConfig.Size, false);
        }

        public async UniTask PutBuilding(Vector2Int gridPosition, BuildingType buildingType)
        {
            if (_isTileChangedComplete == false)
                return;

            _isTileChangedComplete = false;

            await GetTile(gridPosition).PutBuilding(new Building(buildingType));

            _isTileChangedComplete = true;
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

        private IEnumerable<int> GetLineNeighbors(int linePosition)
        {
            for (int i = linePosition - 1; i <= linePosition + 1; i++)
                yield return i;
        }

        private List<SandboxTile> CreateTiles()
        {
            List<SandboxTile> tiles = new();

            foreach (SandboxTileData tileData in _persistentProgressService.Progress.SandboxData.Tiles)
                tiles.Add(new SandboxTile(tileData, _staticDataService));

            return tiles;
        }

        private SandboxTile GetTile(Vector2Int gridPosition) =>
            _tiles.FirstOrDefault(tile => tile.GridPosition == gridPosition);
    }
}
