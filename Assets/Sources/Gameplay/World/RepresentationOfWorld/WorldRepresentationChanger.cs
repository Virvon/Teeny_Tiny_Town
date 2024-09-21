using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.GameplayFactory;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using TileRepresentation = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.TileRepresentation;

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

        private void OnNewBuildingPlaced(Vector2Int gridPosition) =>
            _gameplayMover.PlaceNewBuilding(gridPosition);

        private void OnBuildingRemoved(Vector2Int gridPosition) =>
            _gameplayMover.RemoveBuilding(gridPosition);

        private void OnBuildingReplaced(
            Vector2Int fromGridPosition,
            BuildingType fromBuildingType,
            Vector2Int toGridPosition,
            BuildingType toBuildingType) =>
            _gameplayMover.ReplaceBuilding(fromGridPosition, fromBuildingType, toGridPosition, toBuildingType);

        private async void OnTilesChanged(List<Tile> tiles)
        {
            Debug.Log("updated tiles " + tiles.Count);

            foreach (Tile tile in tiles)
            {
                TileRepresentation tileRepresentation = WorldGenerator.GetTile(tile.GridPosition);
                await tileRepresentation.Change(tile.BuildingType, tile.Ground.Type, tile.Ground.Rotation);
            }

            await _newBuildingPlacePositionHandler.BuildingMarker.TryUpdate(_world.BuildingForPlacing.Type);
            _newBuildingPlacePositionHandler.StartPlacing(WorldGenerator.GetTile(_world.BuildingForPlacing.GridPosition));

            GameplayMoved?.Invoke();
        }
    }
}
