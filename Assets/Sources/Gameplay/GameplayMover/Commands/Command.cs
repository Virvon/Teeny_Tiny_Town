using Assets.Sources.Data;
using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public abstract class Command
    {
        protected readonly IWorldChanger WorldChanger;
        protected readonly TileData[] TileDatas;
        protected readonly IWorldData WorldData;

        private readonly BuildingForPlacingInfo _buildingForPlacing;

        public Command(IWorldChanger worldChanger, IWorldData worldData)
        {
            WorldChanger = worldChanger;
            _buildingForPlacing = WorldChanger.BuildingForPlacing;
            WorldData = worldData;

            TileDatas = WorldData.Tiles.Select(tile => new TileData(tile.GridPosition, tile.BuildingType)).ToArray();
        }

        public abstract void Change();

        public virtual async UniTask Undo()
        {
            WorldData.UpdateTileDatas(TileDatas);

            await WorldChanger.Update(TileDatas, _buildingForPlacing);
            Debug.Log("error in building for placing");
        }
    }
}
