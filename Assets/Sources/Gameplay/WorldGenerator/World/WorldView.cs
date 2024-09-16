using UnityEngine;
using Assets.Sources.Gameplay.WorldGenerator.Comand;
using Zenject;
using Assets.Sources.Gameplay.Tile;
using System.Collections.Generic;
using System;

namespace Assets.Sources.Gameplay.WorldGenerator.World
{
    public class WorldView : MonoBehaviour
    {
        [SerializeField] private BuildingPositionHandler _buildingPositionHandler;
        [SerializeField] private WorldGenerator _worldGenerator;

        private GameplayMover _gameplayMover;
        private World _world;

        [Inject]
        private void Construct(GameplayMover gameplayMover, World world)
        {
            _gameplayMover = gameplayMover;
            _world = world;

            _buildingPositionHandler.BuildingCreated += OnBuildingCreated;
            _world.TilesChanged += OnTilesChanged;
        }

        private void OnDestroy()
        {
            _buildingPositionHandler.BuildingCreated -= OnBuildingCreated;
            _world.TilesChanged += OnTilesChanged;
        }

        private void OnBuildingCreated(Vector2Int gridPosition, BuildingType type)
        {
            _gameplayMover.Move(gridPosition, type);
        }

        private async void OnTilesChanged(List<Vector2Int> tilesGridPosition)
        {
            foreach(Vector2Int gridPosition in tilesGridPosition)
            {
                await _worldGenerator.GetTile(gridPosition).CreateBuilding(_world.GetTile(gridPosition).BuildingType);
            }

            Tile.Tile tile = _worldGenerator.GetTile(_world.BuildingToPlacing.GridPosition);
            Building building = await tile.CreateBuilding(_world.BuildingToPlacing.Type);

            _buildingPositionHandler.Set(building, tile);
        }
    }
}
