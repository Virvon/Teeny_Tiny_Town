using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.GameplayFactory;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Tile = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Tile;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldRepresentationChanger
    {
        private GameplayMover.GameplayMover _gameplayMover;
        private WorldInfrastructure.World _world;
        private NewBuildingPlacePositionHandler _newBuildingPlacePositionHandler;
        private RemovedBuildingPositionHandler _removedBuildingPositionHandler;
        private ReplacedBuildingPositionHandler _replacedBuildingPositionHandler;
        private IGameplayFactory _gameplayFactory;

        public WorldRepresentationChanger(
            GameplayMover.GameplayMover gameplayMover,
            WorldInfrastructure.World world,
            NewBuildingPlacePositionHandler newBuildingPlacePositionHandler,
            RemovedBuildingPositionHandler removedBuildingPositionHandler,
            ReplacedBuildingPositionHandler replacedBuildingPositionHandler,
            IGameplayFactory gameplayFactory)
        {
            _gameplayMover = gameplayMover;
            _world = world;
            _newBuildingPlacePositionHandler = newBuildingPlacePositionHandler;
            _removedBuildingPositionHandler = removedBuildingPositionHandler;
            _replacedBuildingPositionHandler = replacedBuildingPositionHandler;
            _gameplayFactory = gameplayFactory;

            _world.TilesChanged += OnTilesChanged;
            _newBuildingPlacePositionHandler.Placed += OnNewBuildingPlaced;
            _removedBuildingPositionHandler.Removed += OnBuildingRemoved;
            _replacedBuildingPositionHandler.Replaced += OnBuildingReplaced;
        }

        ~WorldRepresentationChanger()
        {
            _newBuildingPlacePositionHandler.Placed -= OnNewBuildingPlaced;
            _world.TilesChanged += OnTilesChanged;
            _removedBuildingPositionHandler.Removed -= OnBuildingRemoved;
            _replacedBuildingPositionHandler.Replaced -= OnBuildingReplaced;
        }

        public event Action GameplayMoved;

        WorldGenerator WorldGenerator => _gameplayFactory.WorldGenerator;

        private void OnNewBuildingPlaced(Vector2Int gridPosition, BuildingType type) =>
            _gameplayMover.PlaceNewBuilding(gridPosition, type);

        private void OnBuildingRemoved(Vector2Int gridPosition) =>
            _gameplayMover.RemoveBuilding(gridPosition);

        private void OnBuildingReplaced(
            Vector2Int fromGridPosition,
            BuildingType fromBuildingType,
            Vector2Int toGridPosition,
            BuildingType toBuildingType) =>
            _gameplayMover.ReplaceBuilding(fromGridPosition, fromBuildingType, toGridPosition, toBuildingType);

        private async void OnTilesChanged(List<Vector2Int> tilesGridPosition)
        {
            foreach (Vector2Int gridPosition in tilesGridPosition)
                await WorldGenerator.GetTile(gridPosition).CreateBuilding(_world.GetTile(gridPosition).BuildingType);

            Tile tile = WorldGenerator.GetTile(_world.BuildingForPlacing.GridPosition);
            Tiles.Building building = await _gameplayFactory.CreateBuilding(_world.BuildingForPlacing.Type, tile.BuildingPoint.position, tile.transform);

            _newBuildingPlacePositionHandler.SetNewBuilding(building, tile);

            GameplayMoved?.Invoke();
        }
    }
}
