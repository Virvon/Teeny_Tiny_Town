using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public abstract class Command
    {
        protected readonly WorldChanger WorldChanger;
        private readonly TileInfrastructureData[] _tileDatas;
        private readonly BuildingForPlacingInfo _buildingForPlacing;

        public Command(WorldChanger world)
        {
            WorldChanger = world;
            _buildingForPlacing = WorldChanger.BuildingForPlacing;
            _tileDatas = WorldChanger.Tiles.Select(tile => new TileInfrastructureData(tile.GridPosition, tile.BuildingType)).ToArray();
        }

        public abstract void Change();

        public virtual async UniTask Undo() =>
            await WorldChanger.Update(_tileDatas, _buildingForPlacing);
    }
}
