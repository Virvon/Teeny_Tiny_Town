using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings
{
    public class Chest : Building
    {
        private readonly uint _reward;
        private readonly Vector2Int _gridPosition;

        public Chest(BuildingType type, IStaticDataService staticDataService, Vector2Int gridPosition)
            : base(type)
        {
            _reward = staticDataService.GetBuilding<ChestConfig>(Type).Reward;
            _gridPosition = gridPosition;
        }

        public override async UniTask CreateRepresentation(TileRepresentation tileRepresentation, bool waitForCompletion)
        {
            ChestRepresentation chestRepresentation = await tileRepresentation.TryChangeBuilding<ChestRepresentation>(Type, waitForCompletion);
            chestRepresentation.Init(_gridPosition, _reward);
        }
    }
}
