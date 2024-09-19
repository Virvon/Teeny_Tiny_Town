using Assets.Sources.Gameplay.World.WorldInfrastructure;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public abstract class Command
    {
        public readonly Building BuildingForPlacing;
        protected readonly World.WorldInfrastructure.World World;
        private readonly TileData[] _tileDatas;

        public Command(World.WorldInfrastructure.World world)
        {
            World = world;
            BuildingForPlacing = World.BuildingForPlacing;
            _tileDatas = World.Tiles.Select(value => new TileData(value.GridPosition, value.BuildingType)).ToArray();
        }

        public abstract void Change();

        public ReadOnlyArray<TileData> TileDatas => _tileDatas;
    }
}
