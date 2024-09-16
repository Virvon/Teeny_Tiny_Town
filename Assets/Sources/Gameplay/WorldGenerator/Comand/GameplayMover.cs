using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Gameplay.WorldGenerator.World;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Gameplay.WorldGenerator.Comand
{
    public class GameplayMover
    {
        private readonly World.World _world;

        private ChangeWorldCommand _lastChangeWorldCommand;

        public GameplayMover(World.World world)
        {
            _world = world;
        }

        public event Action MoveUndoed;

        public void Move(Vector2Int placedBuildingGridPosition, BuildingType placedBuildingType)
        {
            ChangeWorldCommand changeWorldCommand = new(_world, placedBuildingGridPosition, placedBuildingType, _world.BuildingToPlacing);
            _lastChangeWorldCommand = changeWorldCommand;
            changeWorldCommand.Change();
        }

        public void TryUndoMove()
        {
            if (_lastChangeWorldCommand == null)
                return;

            _world.Update(_lastChangeWorldCommand.TileDatas, _lastChangeWorldCommand.BuildingToPlacing);
            _lastChangeWorldCommand = null;
        }
    }

    public class ChangeWorldCommand
    {
        public readonly BuildingModel BuildingToPlacing;

        private readonly World.World _world;
        private readonly TileData[] _tileDatas;
        private readonly Vector2Int _placedBuildingGridPosition;
        private readonly BuildingType _placedBuildingType;

        public ChangeWorldCommand(World.World world, Vector2Int placedBuildingGridPosition, BuildingType placedBuildingType, BuildingModel buildingToPlacing)
        {
            _world = world;
            _placedBuildingGridPosition = placedBuildingGridPosition;
            _placedBuildingType = placedBuildingType;

            _tileDatas = _world.Tiles.Select(value => new TileData(value.GridPosition, value.BuildingType)).ToArray();
            BuildingToPlacing = buildingToPlacing;
        }

        public ReadOnlyArray<TileData> TileDatas => _tileDatas;

        public void Change()
        {
            _world.Update(_placedBuildingGridPosition, _placedBuildingType);
        }
    }

    public class TileData
    {
        public readonly Vector2Int TileGridPosition;
        public readonly BuildingType BuildingType;

        public TileData(Vector2Int tileGridPosition, BuildingType buildingType)
        {
            TileGridPosition = tileGridPosition;
            BuildingType = buildingType;
        }
    }
}
