﻿using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class BuildingMarker : MonoBehaviour
    {
        private IWorldFactory _worldFactory;
        private Building _building;

        [Inject]
        private void Construct(IWorldFactory worldFactory)
        {
            _worldFactory = worldFactory;
        }

        public TileRepresentation MarkedTile { get; private set; }

        public void Mark(TileRepresentation tile)
        {
            transform.position = tile.BuildingPoint.position;
            MarkedTile = tile;
        }

        public void Show()
        {
            if(_building != null)
                _building.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if(_building != null)
                _building.gameObject.SetActive(false);
        }

        public void Replace(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        public async UniTask TryUpdate(BuildingType targetBuildingType)
        {
            if (targetBuildingType == BuildingType.Undefined)
                Debug.LogError("building type can not be undefined");

            if(_building == null || _building.Type != targetBuildingType)
            {
                _building?.Destroy();

                _building = await _worldFactory.CreateBuilding(targetBuildingType, transform.position, transform);
            }
        }

        public class Factory : PlaceholderFactory<string, UniTask<BuildingMarker>>
        {
        } 
    }
}
