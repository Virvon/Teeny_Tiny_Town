using Assets.Sources.Data.Sandbox;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Sandbox
{
    public class SandboxTile
    {
        public readonly Vector2Int GridPosition;

        private readonly SandboxTileData _tileData;
        private readonly IStaticDataService _staticDataService;

        private List<SandboxTile> _adjacentTiles;

        public SandboxTile(SandboxTileData tileData, IStaticDataService staticDataService)
        {
            _tileData = tileData;
            _staticDataService = staticDataService;

            GridPosition = _tileData.GridPosition;

            _adjacentTiles = new();
        }

        public bool IsEmpty => Building == null;
        public SandboxGroundType SandboxGroundType => _tileData.GroundType;
        public Building Building { get; protected set; }
        protected TileRepresentation TileRepresentation { get; private set; }

        public void AddAdjacentTile(SandboxTile adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
        }

        public async UniTask CleanAll()
        {
            if (IsEmpty == false)
                TileRepresentation.DestroyBuilding();

            SetBuilding(null);
            await TileRepresentation.GroundCreator.Create(TileType.RoadGround);
        }

        public async UniTask PutGround(SandboxGroundType sandboxGroundType)
        {
            await RemoveBuilding();

            if (SandboxGroundType == sandboxGroundType)
                return;

            _tileData.GroundType = sandboxGroundType;

            switch (sandboxGroundType)
            {
                case SandboxGroundType.Soil:
                    await TileRepresentation.GroundCreator.Create(TileType.RoadGround);
                    break;
                case SandboxGroundType.WaterSurface:
                    await TileRepresentation.GroundCreator.Create(TileType.WaterSurface);
                    break;
                case SandboxGroundType.TallGround:
                    await TileRepresentation.GroundCreator.Create(TileType.TallGround);
                    break;
                case SandboxGroundType.SoilRoad:
                    await ChangeRoadsInChain(new(), false);
                    break;
                case SandboxGroundType.AsphaltRoad:
                    await ChangeRoadsInChain(new(), false);
                    break;
            }
        }

        public async UniTask ChangeRoadsInChain(List<SandboxTile> countedTiles, bool isWaitedForCreation)
        {
            countedTiles.Add(this);

            if (await TryValidateRoad(_adjacentTiles, IsEmpty, GridPosition) == false)
                return;

            foreach (SandboxTile tile in _adjacentTiles)
            {
                if (tile.SandboxGroundType == SandboxGroundType && countedTiles.Contains(tile) == false)
                    await tile.ChangeRoadsInChain(countedTiles, isWaitedForCreation);
            }

            //await CreateGroundRepresentation(isWaitedForCreation);
        }

        public async UniTask<bool> TryValidateRoad(IReadOnlyList<SandboxTile> adjacentTiles, bool isEmpty, Vector2Int gridPosition)
        {
            if (isEmpty)
            {
                List<Vector2Int> adjacentEmptyTileGridPositions = adjacentTiles
                    .Where(tile => tile.IsEmpty && tile.SandboxGroundType == SandboxGroundType)
                    .Select(tile => tile.GridPosition)
                    .ToList();

                GroundType groundType = SandboxGroundType == SandboxGroundType.SoilRoad ? Services.StaticDataService.Configs.GroundType.Soil : Services.StaticDataService.Configs.GroundType.Asphalt;

                return await TrySet(gridPosition, adjacentEmptyTileGridPositions, groundType);
            }

            return true;
        }

        public async UniTask<bool> TrySet(Vector2Int gridPosition, List<Vector2Int> adjacentGridPosition, GroundType targetGroundType)
        {
            RoadType newRoadType = _staticDataService.GroundsConfig.GetRoadType(gridPosition, adjacentGridPosition, targetGroundType, out GroundRotation rotation);

            await TileRepresentation.GroundCreator.Create(targetGroundType, newRoadType, rotation, false);

            return true;
        }

        public async UniTask PutBuilding(Building building)
        {
            if (building == null)
            {
                await RemoveBuilding();

                return;
            }

            await SetUpBuilding(building);

            if(_tileData.GroundType != SandboxGroundType.TallGround && building.Type != BuildingType.Lighthouse)
            {
                GroundType groundType = _staticDataService.GetGroundType(Building.Type);

                await TileRepresentation.GroundCreator.Create(groundType, RoadType.NonEmpty, GroundRotation.Degrees0, false);

                _tileData.GroundType = SandboxGroundType.Soil;
            }
            else if(building.Type == BuildingType.Lighthouse)
            {
                await TileRepresentation.GroundCreator.Create(TileType.WaterSurface);

                _tileData.GroundType = SandboxGroundType.WaterSurface;
            }
        }

        public async UniTask RemoveBuilding()
        {
            if (IsEmpty)
                return;

            TileRepresentation.DestroyBuilding();
            await Clean();
        }

        private TileType GetTileType()
        {
            switch (_tileData.GroundType)
            {
                case SandboxGroundType.Soil:
                    return TileType.RoadGround;
                case SandboxGroundType.WaterSurface:
                    return TileType.WaterSurface;
                case SandboxGroundType.TallGround:
                    return TileType.TallGround;
                case SandboxGroundType.SoilRoad:
                    return TileType.RoadGround;
                case SandboxGroundType.AsphaltRoad:
                    return TileType.RoadGround;
            }

            return TileType.RoadGround;
        }

        public async UniTask CreateRepresentation(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            

            TileRepresentation = await tileRepresentationCreatable.Create(GridPosition, GetTileType());
            await CreateGroundRepresentation(false);

            if (Building != null)
                await Building.CreateRepresentation(TileRepresentation,false, false);
        }

        protected virtual async UniTask CreateGroundRepresentation(bool isWaitedForCreation)
        {
            await TileRepresentation.GroundCreator.Create(GetTileType());
        }

        protected virtual async UniTask SetUpBuilding(Building building)
        {
            await CreateBuildingRepresentation(building);
        }

        protected virtual UniTask Clean()
        {
            SetBuilding(null);

            return default;
        }

        protected virtual async UniTask CreateBuildingRepresentation(Building building)
        {
            SetBuilding(building);

            await Building.CreateRepresentation(TileRepresentation, false, false);
        }

        protected void SetBuilding(Building building)
        {
            Building = building;
            _tileData.BuildingType = IsEmpty ? BuildingType.Undefined : Building.Type;
        }
    }
}
