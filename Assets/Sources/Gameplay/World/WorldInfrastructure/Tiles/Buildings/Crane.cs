using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings
{
    public class Crane : Building
    {
        private const uint MinTilesCountToMerge = 3;

        private readonly ITileGetable _tileGetable;
        private readonly Vector2Int _gridPosition;

        private TallTile _selfTile;

        public Crane(BuildingType type, ITileGetable tileGetable, Vector2Int gridPosition)
            : base(type)
        {
            _tileGetable = tileGetable;
            _gridPosition = gridPosition;

            Type = GetNewType();
        }

        public override async UniTask CreateRepresentation(TileRepresentation tileRepresentation, bool waitForCompletion)
        {
            await base.CreateRepresentation(tileRepresentation, waitForCompletion);

            if (Type == BuildingType.Crane)
                await _selfTile.RemoveBuilding();
        }

        private BuildingType GetNewType()
        {
            HashSet<BuildingType> aroundBuildingTypes = new();
            _selfTile = _tileGetable.GetTile(_gridPosition) as TallTile;
            BuildingType newType = BuildingType.Undefined;

            foreach (Tile tile in _selfTile.AdjacentTiles)
            {
                if(tile.BuildingType != BuildingType.Undefined)
                    aroundBuildingTypes.Add(tile.BuildingType);
            }

            foreach(BuildingType type in aroundBuildingTypes)
            {
                int chainLength = _selfTile.GetBuildingsChainLength(new(), type);

                if (chainLength >= MinTilesCountToMerge && (int)type > (int)newType)
                    newType = type;
            }

            return newType == BuildingType.Undefined ? BuildingType.Crane : newType;
        }
    }
}
