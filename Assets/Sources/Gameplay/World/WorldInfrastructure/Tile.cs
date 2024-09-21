using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class Tile
    {
        public readonly Vector2Int GridPosition;

        private readonly IStaticDataService _staticDataService;

        private List<Tile> _adjacentTiles;

        public Tile(Vector2Int greedPosition, IStaticDataService staticDataService)
        {
            GridPosition = greedPosition;
            _staticDataService = staticDataService;

            _adjacentTiles = new();
            Ground = new(staticDataService);
        }

        public Ground Ground { get; private set; }
        public BuildingType BuildingType { get; private set; }
        public bool IsEmpty => BuildingType == BuildingType.Undefined;

        public void Init(Tile adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
            TryChangeGroundType();
        }

        public void PutBuilding(BuildingType buildingType)
        {
            BuildingType = buildingType;
        }

        public void PutGround(GroundType type, GroundRotation rotation)
        {
            Ground.Change(type, rotation);
        }

        public void UpdateBuilding()
        {
            BuildingType = _staticDataService.GetMerge(BuildingType).NextBuilding;
        }

        public void RemoveBuilding()
        {
            BuildingType = BuildingType.Undefined;
        }

        public int GetBuildingsChainLength(List<Tile> countedTiles)
        {
            int chainLength = 1;
            countedTiles.Add(this);

            foreach (Tile tile in _adjacentTiles)
            {
                if (BuildingType == tile.BuildingType && countedTiles.Contains(tile) == false)
                    chainLength += tile.GetBuildingsChainLength(countedTiles);
            }

            return chainLength;
        }

        public List<Tile> ChangeGroudsInChain(List<Tile> countedTiles, List<Tile> changedTiles)
        {
            countedTiles.Add(this);
            
            if(TryChangeGroundType())
                changedTiles.Add(this);

            foreach (Tile tile in _adjacentTiles)
            {
                if (tile.IsEmpty && countedTiles.Contains(tile) == false)
                    tile.ChangeGroudsInChain(countedTiles, changedTiles);
            }

            return changedTiles;
        }

        private bool TryChangeGroundType()
        {
            if (IsEmpty)
            {
                List<Vector2Int> adjacentEmptyTileGridPositions = (_adjacentTiles.Where(tile => tile.IsEmpty).Select(tile => tile.GridPosition)).ToList();

                return Ground.TryChange(GridPosition, adjacentEmptyTileGridPositions);
            }
            else
            {
                Ground.SetSoil();

                return true;
            }    
        }
    }
}
