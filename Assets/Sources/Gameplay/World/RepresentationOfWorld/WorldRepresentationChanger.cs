using System.Collections.Generic;
using System;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using UnityEngine;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldRepresentationChanger
    {
        private readonly IWorldChanger _worldChanger;
        private readonly IWorldFactory _worldFactory;
        private readonly BuildingMarker _buildingMarker;

        public WorldRepresentationChanger(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            BuildingMarker buildingMarker)
        {
            _worldChanger = worldChanger;

            _worldChanger.TilesChanged += OnTilesChanged;

            _worldFactory = worldFactory;
            _buildingMarker = buildingMarker;
        }

        ~WorldRepresentationChanger()
        {
            _worldChanger.TilesChanged -= OnTilesChanged;
        }

        public event Action GameplayMoved;

        public TileRepresentation StartTile => WorldGenerator.GetTile(_worldChanger.BuildingForPlacing.GridPosition);

        private WorldGenerator WorldGenerator => _worldFactory.WorldGenerator;

        private async void OnTilesChanged()
        {
            await _buildingMarker.TryUpdate(_worldChanger.BuildingForPlacing.Type);

            GameplayMoved?.Invoke();
        }
    }
}
