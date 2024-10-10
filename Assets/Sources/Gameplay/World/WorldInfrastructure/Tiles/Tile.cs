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

        protected readonly IStaticDataService StaticDataService;

        public Tile(TileType type, Vector2Int greedPosition, IStaticDataService staticDataService, Building building)
        {
            Type = type;
            GridPosition = greedPosition;
            StaticDataService = staticDataService;
            Building = building;
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
            Building = null;
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
            Building = building;

            await Building.CreateRepresentation(TileRepresentation);
        }
    }
}
