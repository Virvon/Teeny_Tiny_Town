using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles
{
    public class TallTile : Tile
    {
        private const uint MinTilesCountToMerge = 3;

        private readonly IWorldData _worldData;
        private readonly IBuildingGivable _buildingGibable;

        private List<TallTile> _adjacentTiles;

        public TallTile(
            TileData tileData,
            TileType type,
            IStaticDataService staticDataService,
            Building building,
            IWorldData worldData,
            IBuildingGivable buildingGibable)
            : base(tileData, type, staticDataService, building)
        {
            _worldData = worldData;

            _adjacentTiles = new();
            _buildingGibable = buildingGibable;
        }

        public IReadOnlyList<TallTile> AdjacentTiles => _adjacentTiles;

        public void AddAdjacentTile(TallTile adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
        }

        protected override async UniTask SetUpBuilding(Building building)
        {
            SetBuilding(building);

            await TryUpdateBuildingsChain(building);
        }

        public int GetBuildingsChainLength(List<TallTile> countedTiles, BuildingType targetBuildingType = default)
        {
            int chainLength = 1;
            countedTiles.Add(this);

            foreach (TallTile tile in _adjacentTiles)
            {
                if(targetBuildingType == default)
                {
                    if (BuildingType == tile.BuildingType && countedTiles.Contains(tile) == false)
                        chainLength += tile.GetBuildingsChainLength(countedTiles);
                }
                else
                {
                    if (targetBuildingType == tile.BuildingType && countedTiles.Contains(tile) == false)
                        chainLength += tile.GetBuildingsChainLength(countedTiles);
                }
            }

            return chainLength;
        }

        private async UniTask TryUpdateBuildingsChain(Building building)
        {
            await CreateBuildingRepresentation(building);

            if (IsEmpty)
                return;

            bool chainCheakCompleted = false;

            while (chainCheakCompleted == false)
            {
                List<TallTile> countedTiles = new();

                if (GetBuildingsChainLength(countedTiles) >= MinTilesCountToMerge && await TryUpgradeBuilding())
                {
                    List<TallTile> tilesForRemoveBuildings = countedTiles;
                    tilesForRemoveBuildings.Remove(this);

                    foreach (Tile tile in countedTiles)
                        await tile.RemoveBuilding();
                }
                else
                {
                    chainCheakCompleted = true;
                }
            }
        }

        protected IReadOnlyList<TAdjacentTile> GetAdjacentTiles<TAdjacentTile>()
        {
            List<TAdjacentTile> adjacentTiles = new();

            foreach(TallTile tile in _adjacentTiles)
            {
                if(tile is TAdjacentTile adjacentTile)
                    adjacentTiles.Add(adjacentTile);
            }

            return adjacentTiles;
        }

        private async UniTask<bool> TryUpgradeBuilding()
        {
            if (StaticDataService.AvailableForConstructionBuildingsConfig.TryFindeNextBuilding(BuildingType, out BuildingType nextBuildingType))
            {
                await CreateBuildingRepresentation(_buildingGibable.GetBuilding(nextBuildingType, GridPosition));
                _worldData.TryAddBuildingTypeForCreation(nextBuildingType, StaticDataService.AvailableForConstructionBuildingsConfig.requiredCreatedBuildingsToAddNext, StaticDataService);

                return true;
            }

            return false;
        }
    }
}

