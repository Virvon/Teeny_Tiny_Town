using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldRepresentationChanger
    {
        private readonly GameplayMover.GameplayMover _gameplayMover;
        private readonly WorldChanger _world;
        private readonly NewBuildingPlacePositionHandler _newBuildingPlacePositionHandler;
        private readonly RemovedBuildingPositionHandler _removedBuildingPositionHandler;
        private readonly ReplacedBuildingPositionHandler _replacedBuildingPositionHandler;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IWorldFactory _worldFactory;

        public WorldRepresentationChanger(
            GameplayMover.GameplayMover gameplayMover,
            WorldChanger world,
            NewBuildingPlacePositionHandler newBuildingPlacePositionHandler,
            RemovedBuildingPositionHandler removedBuildingPositionHandler,
            ReplacedBuildingPositionHandler replacedBuildingPositionHandler,
            IGameplayFactory gameplayFactory,
            IWorldFactory worldFactory)
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
            _worldFactory = worldFactory;
        }

        ~WorldRepresentationChanger()
        {
            _newBuildingPlacePositionHandler.Placed -= OnNewBuildingPlaced;
            _world.TilesChanged += OnTilesChanged;
            _removedBuildingPositionHandler.Removed -= OnBuildingRemoved;
            _replacedBuildingPositionHandler.Replaced -= OnBuildingReplaced;
        }

        public event Action GameplayMoved;

        WorldGenerator WorldGenerator => _worldFactory.WorldGenerator;

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
            foreach (Tile tile in tiles)
            {
                TileRepresentation tileRepresentation = WorldGenerator.GetTile(tile.GridPosition);
                await tileRepresentation.Change(tile.BuildingType, tile.Ground.Type, tile.Ground.RoadType, tile.Ground.Rotation);
            }

            await _newBuildingPlacePositionHandler.BuildingMarker.TryUpdate(_world.BuildingForPlacing.Type);
            _newBuildingPlacePositionHandler.StartPlacing(WorldGenerator.GetTile(_world.BuildingForPlacing.GridPosition));

            GameplayMoved?.Invoke();
        }
    }
}
