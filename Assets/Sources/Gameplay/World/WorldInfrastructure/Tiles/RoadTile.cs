using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
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

        public override async UniTask PutBuilding(BuildingType buildingType)
        {
            BuildingType = buildingType;

            GetAllUpdatedByBuildingTiles();
            await TileRepresentation.TryChangeBuilding(BuildingType);
        }

        public override void RemoveBuilding()
        {
            base.RemoveBuilding();
        }

        public void ValidateRoadType() =>
            Ground.TryValidateRoad(_adjacentTiles, IsEmpty, GridPosition);

        public void ValidateGroundType() =>
            Ground.TryUpdate(_aroundTiles, IsEmpty);


        private void GetAllUpdatedByBuildingTiles()
        {
            List<RoadTile> changedTiles = new() { this };

            if (IsEmpty)
                return;

            bool chainCheakCompleted = false;

            while (chainCheakCompleted == false)
            {
                List<RoadTile> countedTiles = new();

                if (GetBuildingsChainLength(countedTiles) >= MinTilesCountToMerge)
                {
                    changedTiles.Union(countedTiles);

                    List<RoadTile> tilesForRemoveBuildings = countedTiles;
                    tilesForRemoveBuildings.Remove(this);

                    foreach (Tile tile in countedTiles)
                        tile.RemoveBuilding();

                    UpdateBuilding();
                }
                else
                {
                    chainCheakCompleted = true;
                }
            }
        }

        private void UpdateBuilding()
        {
            BuildingType = StaticDataService.GetBuilding<BuildingConfig>(BuildingType).NextBuilding;
            ChangeGroundType();
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
            bool x = Ground.TryValidateRoad(_adjacentTiles, IsEmpty, GridPosition);

            Debug.Log(GridPosition + " " + x + " " + Ground.RoadType);

            if (x == false)
                return;

            foreach (RoadTile tile in _adjacentTiles)
            {
                if (tile.IsEmpty && countedTiles.Contains(tile) == false)
                    await tile.ChangeRoadsInChain(countedTiles);
            }
        }

       
        public void ChangeGroundsInChain(List<RoadTile> countedTiles)
        {
            InspectCount++;

            countedTiles.Add(this);

            if (Ground.TryUpdate(_aroundTiles, IsEmpty) == false)
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

        private bool TryChangeGroundType()
        {
            if (IsEmpty == false)
                return false;

            GroundType groundType = GroundType.Soil;

            foreach (RoadTile tile in _aroundTiles)
            {
                if (tile.IsEmpty == false && (int)tile.Ground.Type > (int)groundType)
                    groundType = tile.Ground.Type;
            }

            bool isChanged = groundType != Ground.Type;

            Ground.ChangeGroundType(groundType);

            return isChanged;
        }


        private void ChangeGroundType()
        {
            Ground.ChangeGroundType(IsEmpty ? GroundType.Soil : StaticDataService.GetGroundType(BuildingType));
        }
    }
}
