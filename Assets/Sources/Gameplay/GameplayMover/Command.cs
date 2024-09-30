using Assets.Sources.Gameplay.World.WorldInfrastructure;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public abstract class Command
    {
        public readonly Building BuildingForPlacing;
        protected readonly WorldChanger World;
        private readonly TileInfrastructureData[] _tileDatas;

        public Command(World.WorldInfrastructure.WorldChanger world)
        {
            World = world;
            BuildingForPlacing = World.BuildingForPlacing;
            _tileDatas = World.Tiles.Select(tile => new TileInfrastructureData(tile.GridPosition, tile.BuildingType, tile.Ground.RoadType, tile.Ground.Rotation)).ToArray();
        }

        public abstract void Change();

        public ReadOnlyArray<TileInfrastructureData> TileDatas => _tileDatas;
    }
}
