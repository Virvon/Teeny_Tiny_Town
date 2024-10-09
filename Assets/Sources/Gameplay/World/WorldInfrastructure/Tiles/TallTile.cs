using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StaticDataService;
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

        private List<TallTile> _adjacentTiles;

        public TallTile(TileType type, Vector2Int greedPosition, IStaticDataService staticDataService, BuildingType buildingType, WorldData worldData) : base(type, greedPosition, staticDataService, buildingType)
        {
            _worldData = worldData;

            _adjacentTiles = new();
        }

        public void AddAdjacentTile(TallTile adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
        }

        public override async UniTask PutBuilding(BuildingType buildingType) =>
            await TryUpdateBuildingsChain(buildingType);

        public int GetBuildingsChainLength(List<TallTile> countedTiles)
        {
            int chainLength = 1;
            countedTiles.Add(this);

            foreach (TallTile tile in _adjacentTiles)
            {
                if (BuildingType == tile.BuildingType && countedTiles.Contains(tile) == false)
                    chainLength += tile.GetBuildingsChainLength(countedTiles);
            }

            return chainLength;
        }

        private async UniTask TryUpdateBuildingsChain(BuildingType buildingType)
        {
            await CreateBuilding(buildingType);

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
                        tile.RemoveBuilding();
                    await UpgradeBuilding();
                }
                else
                {
                    chainCheakCompleted = true;
                }
            }
        }

        protected virtual async UniTask CreateBuilding(BuildingType type)
        {
            BuildingType = type;

            await TileRepresentation.TryChangeBuilding(BuildingType);
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

            await CreateBuilding(nextBuildingType);
        }
    }
}

