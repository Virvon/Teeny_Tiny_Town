﻿using Assets.Sources.Gameplay.World.WorldInfrastructure;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class PlaceNewBuildingCommand : Command
    {
        private readonly Vector2Int _placedBuildingGridPosition;
        private readonly BuildingType _placedBuildingType;

        public PlaceNewBuildingCommand(World.WorldInfrastructure.World world, Vector2Int placedBuildingGridPosition)
            : base(world)
        {
            _placedBuildingGridPosition = placedBuildingGridPosition;
            _placedBuildingType = world.BuildingForPlacing.Type;
        }

        public override void Change() =>
            World.PlaceNewBuilding(_placedBuildingGridPosition, _placedBuildingType);
    }
}