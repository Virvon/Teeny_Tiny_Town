﻿using System.Collections.Generic;
using Assets.Sources.Data;
using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles
{
    public class RoadTile : TallTile
    {
        private List<RoadTile> _aroundTiles;

        public RoadTile(
            TileData tileData,
            TileType type,
            IStaticDataService staticDataService,
            Building building,
            IWorldData worldData,
            IBuildingGivable buildingGivable,
            IPersistentProgressService persistentProgressService)
            : base(tileData, type, staticDataService, building, worldData, buildingGivable, persistentProgressService)
        {
            Ground = new (StaticDataService, StaticDataService.GetGroundType(BuildingType));

            _aroundTiles = new ();
        }

        public Ground Ground { get; private set; }

        public void ValidateRoadType() =>
            Ground.TryValidateRoad(GetAdjacentTiles<RoadTile>(), IsEmpty, GridPosition);

        public void ValidateGroundType() =>
            Ground.TryTakeAroundTilesGroundType(_aroundTiles, IsEmpty);

        public void AddAroundTile(RoadTile aroundTile)
        {
            _aroundTiles.Add(aroundTile);
        }

        public async UniTask ChangeRoadsInChain(List<RoadTile> countedTiles, bool isWaitedForCreation)
        {
            countedTiles.Add(this);

            if (Ground.TryValidateRoad(GetAdjacentTiles<RoadTile>(), IsEmpty, GridPosition) == false)
                return;

            foreach (RoadTile tile in GetAdjacentTiles<RoadTile>())
            {
                if (tile.IsEmpty && countedTiles.Contains(tile) == false)
                    await tile.ChangeRoadsInChain(countedTiles, isWaitedForCreation);
            }

            await CreateGroundRepresentation(isWaitedForCreation);
        }

        public void ChangeGroundsInChain(List<RoadTile> countedTiles, bool isSelfTile = false)
        {
            countedTiles.Add(this);

            if (Ground.TryTakeAroundTilesGroundType(_aroundTiles, IsEmpty) == false && isSelfTile == false)
                return;

            foreach (RoadTile tile in _aroundTiles)
            {
                if (tile.IsEmpty && countedTiles.Contains(tile) == false)
                    tile.ChangeGroundsInChain(countedTiles);
            }
        }

        protected override async UniTask CreateGroundRepresentation(bool isWaitedForCreation) =>
            await TileRepresentation.GroundCreator.Create(Ground.Type, Ground.RoadType, Ground.Rotation, isWaitedForCreation);

        protected override async UniTask CreateBuildingRepresentation(Building building)
        {
            SetBuilding(building);
            await ValidateTilesInChain(false);
            await Building.CreateRepresentation(TileRepresentation, true, true);
        }

        private async UniTask ValidateTilesInChain(bool isWaitedForRoadCreation)
        {
            if (Ground.TryUpdate(BuildingType))
                ChangeGroundsInChain(new (), true);

            await ChangeRoadsInChain(new (), isWaitedForRoadCreation);
        }

        public override async UniTask CleanAll(bool isAnimate)
        {
            await base.CleanAll(isAnimate);

            Ground.Clean();
            await TileRepresentation.GroundCreator.Create(Ground.Type, Ground.RoadType, Ground.Rotation, isAnimate);
        }

        public override async UniTask UpdateBuilding(Building building, IBuildingsUpdatable buildingsUpdatable, bool isAnimate)
        {
            if (building == null)
                return;

            SetBuilding(building);
            await Building.CreateRepresentation(TileRepresentation, true, false);

            buildingsUpdatable.UpdateFinished += async () => await ValidateTilesInChain(true);
        }

        protected override async UniTask Clean()
        {
            await base.Clean();

            Ground.SetEmpty(_aroundTiles);
            ChangeGroundsInChain(new (), true);
            await ChangeRoadsInChain(new (), false);
        }
    }
}