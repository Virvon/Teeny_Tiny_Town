using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public interface IWorldChanger : IBuildingGivable, ITileGetable
    {
        event Action TilesChanged;

        IReadOnlyList<Tile> Tiles { get; }

        UniTask Generate(ITileRepresentationCreatable tileRepresentationCreatable);
        UniTask PlaceNewBuilding(Vector2Int gridPosition, BuildingType buildingType);
        UniTask RemoveBuilding(Vector2Int destroyBuildingGridPosition);
        UniTask ReplaceBuilding(Vector2Int fromBuildingGridPosition, BuildingType fromBuildingType, Vector2Int toBuildingGridPosition, BuildingType toBuildingType);
        void Start();
        UniTask Update(ReadOnlyArray<TileData> tileDatas);
    }
}