﻿using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles
{
    public class RoadTile : TallTile
    {
        private List<RoadTile> _aroundTiles;

        public RoadTile(
            TileData tileData,
            TileType type,
            IStaticDataService staticDataService,
            Building building, IWorldData worldData,
            IBuildingGivable buildingGivable)
            : base(tileData, type, staticDataService, building, worldData, buildingGivable)
        {
            Ground = new(StaticDataService, StaticDataService.GetGroundType(BuildingType));

            _aroundTiles = new();
        }

        public Ground Ground { get; private set; }

        public override async UniTask RemoveBuilding()
        {
            await base.RemoveBuilding();

            Ground.SetEmpty(_aroundTiles);
            ChangeGroundsInChain(new(), true);
            await ChangeRoadsInChain(new());
        }

        public void ValidateRoadType() =>
            Ground.TryValidateRoad(GetAdjacentTiles<RoadTile>(), IsEmpty, GridPosition);

        public void ValidateGroundType() =>
            Ground.TryTakeAroundTilesGroundType(_aroundTiles, IsEmpty);

        public void AddAroundTile(RoadTile aroundTile)
        {
            _aroundTiles.Add(aroundTile);
        }

        public async UniTask ChangeRoadsInChain(List<RoadTile> countedTiles)
        {
            countedTiles.Add(this);

            if (Ground.TryValidateRoad(GetAdjacentTiles<RoadTile>(), IsEmpty, GridPosition) == false)
                return;

            foreach (RoadTile tile in GetAdjacentTiles<RoadTile>())
            {
                if (tile.IsEmpty && countedTiles.Contains(tile) == false)
                    await tile.ChangeRoadsInChain(countedTiles);
            }

            await CreateGroundRepresentation();
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

        protected override async UniTask CreateGroundRepresentation() =>
            await TileRepresentation.GroundCreator.Create(Ground.Type, Ground.RoadType, Ground.Rotation);

        protected override async UniTask CreateBuildingRepresentation(Building building)
        {
            SetBuilding(building);

            if (Ground.TryUpdate(Building.Type))
                ChangeGroundsInChain(new(), true);

            await ChangeRoadsInChain(new());
            await Building.CreateRepresentation(TileRepresentation);
        }
    }
}

