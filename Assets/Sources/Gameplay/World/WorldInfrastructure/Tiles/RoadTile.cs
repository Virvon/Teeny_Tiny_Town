using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles
{
    public class RoadTile : Tile
    {
        private const uint MinTilesCountToMerge = 3;

        private List<RoadTile> _adjacentTiles;
        private List<RoadTile> _aroundTiles;

        public int InspectCount;

        public RoadTile(TileType type, Vector2Int greedPosition,  IStaticDataService staticDataService, BuildingType buildingType)
            : base(type, greedPosition, staticDataService, buildingType)
        {
            Ground = new(StaticDataService, StaticDataService.GetGroundType(buildingType));

            _adjacentTiles = new();
            _aroundTiles = new();
        }

        public Ground Ground { get; private set; }

        public override async UniTask PutBuilding(BuildingType buildingType) =>
            await TryUpdateBuildingsChain(buildingType);

        public override async void RemoveBuilding()
        {
            base.RemoveBuilding();

            Ground.SetEmpty(_aroundTiles);
            ChangeGroundsInChain(new(), true);
            await ChangeRoadsInChain(new());
        }

        public void ValidateRoadType() =>
            Ground.TryValidateRoad(_adjacentTiles, IsEmpty, GridPosition);

        public void ValidateGroundType() =>
            Ground.TryTakeAroundTilesGroundType(_aroundTiles, IsEmpty);


        private async UniTask TryUpdateBuildingsChain(BuildingType buildingType)
        {
            await CreateBuilding(buildingType);

            if (IsEmpty)
                return;

            bool chainCheakCompleted = false;

            while (chainCheakCompleted == false)
            {
                List<RoadTile> countedTiles = new();

                if (GetBuildingsChainLength(countedTiles) >= MinTilesCountToMerge)
                {
                    List<RoadTile> tilesForRemoveBuildings = countedTiles;
                    tilesForRemoveBuildings.Remove(this);

                    foreach (Tile tile in countedTiles)
                        tile.RemoveBuilding();

                    await UpgradeBuilding();
                }
                else
                {
                    chainCheakCompleted = true;
                }
            }

            
        }

        private async UniTask UpgradeBuilding() =>
            await CreateBuilding(StaticDataService.GetBuilding<BuildingConfig>(BuildingType).NextBuilding);

        private async UniTask CreateBuilding(BuildingType type)
        {
            BuildingType = type;

            if (Ground.TryUpdate(BuildingType))
                ChangeGroundsInChain(new(), true);

            await ChangeRoadsInChain(new());

            await TileRepresentation.TryChangeBuilding(BuildingType);
        }

        public void AddAdjacentTile(RoadTile adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
        }

        public void AddAroundTile(RoadTile aroundTile)
        {
            _aroundTiles.Add(aroundTile);
        }

        public int GetBuildingsChainLength(List<RoadTile> countedTiles)
        {
            InspectCount++;

            int chainLength = 1;
            countedTiles.Add(this);

            foreach (RoadTile tile in _adjacentTiles)
            {
                if (BuildingType == tile.BuildingType && countedTiles.Contains(tile) == false)
                    chainLength += tile.GetBuildingsChainLength(countedTiles);
            }

            return chainLength;
        }

        public async UniTask ChangeRoadsInChain(List<RoadTile> countedTiles)
        {
            InspectCount++;

            countedTiles.Add(this);

            if (Ground.TryValidateRoad(_adjacentTiles, IsEmpty, GridPosition) == false)
                return;

            foreach (RoadTile tile in _adjacentTiles)
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

        private bool TryChangeRoad()
        {
            if (IsEmpty)
            {
                List<Vector2Int> adjacentEmptyTileGridPositions = _adjacentTiles.Where(tile => tile.IsEmpty && tile.Ground.Type == Ground.Type).Select(tile => tile.GridPosition).ToList();

                return Ground.TrySet(GridPosition, adjacentEmptyTileGridPositions, Ground.Type);
            }
            else
            {
                Ground.SetNonEmpty();

                return true;
            }
        }


        //private bool TryChangeGroundType()
        //{
        //    Ground.ChangeGroundType(IsEmpty ? GroundType.Soil : StaticDataService.GetGroundType(BuildingType));
        //}
    }
}
