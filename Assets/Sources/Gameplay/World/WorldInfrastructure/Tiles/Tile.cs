using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles
{
    public class Tile
    {
        public readonly TileType Type;
        public readonly Vector2Int GridPosition;

        protected readonly TileData TileData;
        protected readonly IStaticDataService StaticDataService;

        public Tile(TileData tileData, TileType type, IStaticDataService staticDataService, Building building)
        {
            TileData = tileData;
            Type = type;
            StaticDataService = staticDataService;
            Building = building;

            GridPosition = TileData.GridPosition;
        }

        public Building Building { get; protected set; }
        public bool IsEmpty => Building == null;

        protected TileRepresentation TileRepresentation { get; private set; }
        public BuildingType BuildingType => IsEmpty ? BuildingType.Undefined : Building.Type;

        public async UniTask CreateRepresentation(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            TileRepresentation = await tileRepresentationCreatable.Create(GridPosition);
            await CreateGroundRepresentation();

            if(Building != null)
                await Building.CreateRepresentation(TileRepresentation);
        }

        public async UniTask PutBuilding(Building building)
        {
            if (building == null)
            {
                await RemoveBuilding();

                return;
            }

            await SetUpBuilding(building);
        }

        public void Clean()
        {
            if (IsEmpty)
                return;

            Building.Destroy(TileRepresentation);
            SetBuilding(null);
        }

        public void Destroy()
        {
            TileRepresentation.Destroy();
        }

        public virtual UniTask RemoveBuilding()
        {
            Clean();

            return default;
        }

        protected virtual async UniTask SetUpBuilding(Building building)
        {
            await CreateBuildingRepresentation(building);
        }

        protected virtual async UniTask CreateGroundRepresentation()
        {
            await TileRepresentation.GroundCreator.Create(Type);
        }

        protected virtual async UniTask CreateBuildingRepresentation(Building building)
        {
            SetBuilding(building);

            await Building.CreateRepresentation(TileRepresentation);
        }

        protected void SetBuilding(Building building)
        {
            Building = building;
            TileData.BuildingType = IsEmpty ? BuildingType.Undefined : Building.Type;
        }
    }
}
