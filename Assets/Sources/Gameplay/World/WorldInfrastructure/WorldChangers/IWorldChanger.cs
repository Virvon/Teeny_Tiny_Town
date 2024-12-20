﻿using System;
using System.Collections.Generic;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public interface IWorldChanger : IBuildingGivable, ITileGetable
    {
        event Action TilesChanged;
        event Action<BuildingType> BuildingPlaced;
        event Action<Vector2Int, bool> CenterChanged;

        IReadOnlyList<Tile> Tiles { get; }

        UniTask Generate(ITileRepresentationCreatable tileRepresentationCreatable);
        UniTask PlaceNewBuilding(Vector2Int gridPosition, BuildingType buildingType);
        UniTask RemoveBuilding(Vector2Int destroyBuildingGridPosition);
        UniTask ReplaceBuilding(Vector2Int fromBuildingGridPosition, BuildingType fromBuildingType, Vector2Int toBuildingGridPosition, BuildingType toBuildingType);
        void Start();
        UniTask Update(bool isAnimate);
    }
}