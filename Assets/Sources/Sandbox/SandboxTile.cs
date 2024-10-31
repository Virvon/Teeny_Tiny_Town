using Assets.Sources.Data.Sandbox;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Sandbox
{
    public class SandboxTile
    {
        private readonly TileType _type;
        private readonly SandboxTileData _tileData;
        private readonly Vector2Int _gridPosition;

        public SandboxTile(TileType type, SandboxTileData tileData)
        {
            _type = type;
            _tileData = tileData;

            _gridPosition = _tileData.GridPosition;
        }

        public bool IsEmpty => Building == null;
        public Building Building { get; protected set; }
        protected TileRepresentation TileRepresentation { get; private set; }

        public async UniTask PutBuilding(Building building)
        {
            if (building == null)
            {
                await RemoveBuilding();

                return;
            }

            await SetUpBuilding(building);
        }

        public async UniTask RemoveBuilding()
        {
            if (IsEmpty)
                return;

            TileRepresentation.DestroyBuilding();
            await Clean();
        }

        public async UniTask CreateRepresentation(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            TileRepresentation = await tileRepresentationCreatable.Create(_gridPosition, _type);
            await CreateGroundRepresentation(false);

            if (Building != null)
                await Building.CreateRepresentation(TileRepresentation, true);
        }

        protected virtual async UniTask CreateGroundRepresentation(bool isWaitedForCreation)
        {
            await TileRepresentation.GroundCreator.Create(_type);
        }

        protected virtual async UniTask SetUpBuilding(Building building)
        {
            await CreateBuildingRepresentation(building);
        }

        protected virtual UniTask Clean()
        {
            SetBuilding(null);

            return default;
        }

        protected virtual async UniTask CreateBuildingRepresentation(Building building)
        {
            SetBuilding(building);

            await Building.CreateRepresentation(TileRepresentation, true);
        }

        protected void SetBuilding(Building building)
        {
            Building = building;
            _tileData.BuildingType = IsEmpty ? BuildingType.Undefined : Building.Type;
        }
    }
}
