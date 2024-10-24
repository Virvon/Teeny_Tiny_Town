﻿using System.Collections.Generic;
using Random = UnityEngine.Random;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Data.WorldDatas;
using System;
using System.Linq;
using UnityEngine;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing
{
    public class NextBuildingForPlacingCreator
    {
        private readonly IWorldData _worldData;
        private readonly WorldStateMachine _worldStateMachine;

        public NextBuildingForPlacingCreator(IWorldData worldData, WorldStateMachine worldStateMachine)
        {
            _worldData = worldData;
            _worldStateMachine = worldStateMachine;
        }

        public BuildingsForPlacingData BuildingsForPlacingData { get; private set; }

        public event Action<BuildingsForPlacingData> DataChanged;
        public event Action NoMoreEmptyTiles;

        public void CreateData(IReadOnlyList<Tile> tiles)
        {
            BuildingsForPlacingData = new(CreateBuildingType(), CreateBuildingType());

            FindTileToPlacing(tiles);

            DataChanged?.Invoke(BuildingsForPlacingData);
        }

        public void Update(BuildingsForPlacingData newData)
        {
            BuildingsForPlacingData = newData;

            DataChanged?.Invoke(BuildingsForPlacingData);
        }

        public void ChangeCurrentBuildingForPlacing(BuildingType type)
        {
            BuildingsForPlacingData.CurrentBuildingType = type;

            DataChanged?.Invoke(BuildingsForPlacingData);
        }

        public void MoveToNextBuilding()
        {
            BuildingsForPlacingData.MoveToNext();
            BuildingsForPlacingData.NextBuildingType = CreateBuildingType();

            DataChanged?.Invoke(BuildingsForPlacingData);
        }

        public void MoveToNextBuilding(IReadOnlyList<Tile> tiles)
        {
            FindTileToPlacing(tiles);
            MoveToNextBuilding();
        }

        private BuildingType CreateBuildingType()
        {
            IReadOnlyList<BuildingType> availableBuildingTypes = _worldData.AvailableBuildingsForCreation;

            return availableBuildingTypes[Random.Range(0, availableBuildingTypes.Count)];
        }


        private void FindTileToPlacing(IReadOnlyList<Tile> tiles)
        {
            if(tiles.Any(tile => tile.IsEmpty) == false)
            {
                NoMoreEmptyTiles?.Invoke();
                return;
            }    

            bool isPositionFree = false;

            while (isPositionFree == false)
            {
                Tile tile = tiles[Random.Range(0, tiles.Count)];

                if (tile.IsEmpty)
                {
                    BuildingsForPlacingData.StartGridPosition = tile.GridPosition;
                    isPositionFree = true;
                }
            }
        }
    }
}
