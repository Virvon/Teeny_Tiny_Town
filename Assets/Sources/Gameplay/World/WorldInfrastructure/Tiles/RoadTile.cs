using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles
{
    public class RoadTile : TallTile
    {
        private List<RoadTile> _aroundTiles;

        public int InspectCount;

        public RoadTile(TileType type, Vector2Int greedPosition,  IStaticDataService staticDataService, BuildingType buildingType, WorldData worldData)
            : base(type, greedPosition, staticDataService, buildingType, worldData)
        {
            Ground = new(StaticDataService, StaticDataService.GetGroundType(buildingType));

            _aroundTiles = new();
        }

        public Ground Ground { get; private set; }

        public override async void RemoveBuilding()
        {
            base.RemoveBuilding();

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
            InspectCount++;

            countedTiles.Add(this);

            if (Ground.TryValidateRoad(GetAdjacentTiles<RoadTile>(), IsEmpty, GridPosition) == false)
                return;

            foreach (RoadTile tile in GetAdjacentTiles<RoadTile>())
            {
                if (tile.IsEmpty && countedTiles.Contains(tile) == false)
                    await tile.ChangeRoadsInChain(countedTiles);
            }

            await TileRepresentation.GroundCreator.Create(Ground.Type, Ground.RoadType, Ground.Rotation);
        }

        public void ChangeGroundsInChain(List<RoadTile> countedTiles, bool isSelfTile = false)
        {
            InspectCount++;

            countedTiles.Add(this);

            if (Ground.TryTakeAroundTilesGroundType(_aroundTiles, IsEmpty) == false && isSelfTile == false)
                return;

            foreach (RoadTile tile in _aroundTiles)
            {
                if (tile.IsEmpty && countedTiles.Contains(tile) == false)
                    tile.ChangeGroundsInChain(countedTiles);
            }

        }

        protected override async UniTask CreateGroundRepresentation()
        {
            await TileRepresentation.GroundCreator.Create(Ground.Type, Ground.RoadType, Ground.Rotation);
        }

        protected override async UniTask CreateBuilding(BuildingType type)
        {
            BuildingType = type;

            if (Ground.TryUpdate(BuildingType))
                ChangeGroundsInChain(new(), true);

            await ChangeRoadsInChain(new());

            await TileRepresentation.TryChangeBuilding(BuildingType);
        }
    }
}

