using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
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

        private readonly WorldData _worldData;
        private readonly IBuildingGivable _buildingGibable;

        private List<TallTile> _adjacentTiles;

        public TallTile(TileType type, Vector2Int greedPosition, IStaticDataService staticDataService, Building building, WorldData worldData, IBuildingGivable buildingGibable)
            : base(type, greedPosition, staticDataService, building)
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
            Building = building;

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

                if (GetBuildingsChainLength(countedTiles) >= MinTilesCountToMerge)
                {
                    List<TallTile> tilesForRemoveBuildings = countedTiles;
                    tilesForRemoveBuildings.Remove(this);

                    foreach (Tile tile in countedTiles)
                        await tile.RemoveBuilding();

                    await UpgradeBuilding();
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

        private async UniTask UpgradeBuilding()
        {
            BuildingType nextBuildingType = StaticDataService.AvailableForConstructionBuildingsConfig.FindNextBuilding(BuildingType);

            if (_worldData.TryAddBuildingTypeForCreation(nextBuildingType, StaticDataService.AvailableForConstructionBuildingsConfig.requiredCreatedBuildingsToAddNext))
                _worldData.AddNextBuildingTypeForCreation(StaticDataService.AvailableForConstructionBuildingsConfig.FindNextBuilding(StaticDataService.AvailableForConstructionBuildingsConfig.FindNextBuilding(nextBuildingType)));

            await CreateBuildingRepresentation(_buildingGibable.GetBuilding(nextBuildingType, GridPosition));
        }
    }
}

