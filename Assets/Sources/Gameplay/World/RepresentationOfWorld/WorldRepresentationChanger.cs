using System.Collections.Generic;
using System;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldRepresentationChanger
    {
        private readonly WorldChanger _world;
        private readonly IWorldFactory _worldFactory;
        private readonly BuildingMarker _buildingMarker;

        public WorldRepresentationChanger(
            WorldChanger world,
            IWorldFactory worldFactory,
            BuildingMarker buildingMarker)
        {
            _world = world;

            _world.TilesChanged += OnTilesChanged;

            _worldFactory = worldFactory;
            _buildingMarker = buildingMarker;
        }

        ~WorldRepresentationChanger()
        {
            _world.TilesChanged -= OnTilesChanged;
        }

        public event Action GameplayMoved;

        public TileRepresentation StartTile => WorldGenerator.GetTile(_world.BuildingForPlacing.GridPosition);

        private WorldGenerator WorldGenerator => _worldFactory.WorldGenerator;

        private async void OnTilesChanged(List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                TileRepresentation tileRepresentation = WorldGenerator.GetTile(tile.GridPosition);
                await tileRepresentation.Change(tile.BuildingType, tile.Ground.Type, tile.Ground.RoadType, tile.Ground.Rotation);
            }

            await _buildingMarker.TryUpdate(_world.BuildingForPlacing.Type);

            GameplayMoved?.Invoke();
        }
    }
}
