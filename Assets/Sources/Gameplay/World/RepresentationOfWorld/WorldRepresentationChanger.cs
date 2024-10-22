using System.Collections.Generic;
using System;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using UnityEngine;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldRepresentationChanger
    {
        private readonly IWorldChanger _worldChanger;
        private readonly IWorldFactory _worldFactory;
        private readonly NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;

        public WorldRepresentationChanger(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            _worldChanger = worldChanger;

            _worldChanger.TilesChanged += OnTilesChanged;

            _worldFactory = worldFactory;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
        }

        ~WorldRepresentationChanger()
        {
            _worldChanger.TilesChanged -= OnTilesChanged;
        }

        public event Action GameplayMoved;

        public TileRepresentation StartTile => WorldGenerator.GetTile(_nextBuildingForPlacingCreator.BuildingsForPlacingData.StartGridPosition);

        private WorldGenerator WorldGenerator => _worldFactory.WorldGenerator;

        private void OnTilesChanged()
        {
            GameplayMoved?.Invoke();
        }
    }
}
