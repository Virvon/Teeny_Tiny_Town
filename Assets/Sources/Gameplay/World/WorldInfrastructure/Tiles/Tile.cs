using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
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
        public BuildingType BuildingType => IsEmpty ? BuildingType.Undefined : Building.Type;
        protected TileRepresentation TileRepresentation { get; private set; }

        public async UniTask CreateRepresentation(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            TileRepresentation = await tileRepresentationCreatable.Create(GridPosition, Type);
            await CreateGroundRepresentation(false);

            if(Building != null)
                await Building.CreateRepresentation(TileRepresentation, true, true);
        }

        public virtual async UniTask UpdateBuilding(Building building, IBuildingsUpdatable buildingsUpdatable, bool isAnimate)
        {
            if (building == null)
                return;

            await SetUpBuilding(building);
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

        public void Destroy()
        {
            TileRepresentation.Destroy();
        }

        public virtual async UniTask CleanAll(bool isAnimate)
        {
            if(IsEmpty == false)
            {
                if (isAnimate)
                    await TileRepresentation.AnimateDestroyBuilding();
                else
                    TileRepresentation.DestroyBuilding();
            }

            SetBuilding(null);
        }

        public async UniTask RemoveBuilding()
        {
            if (IsEmpty)
                return;

            TileRepresentation.DestroyBuilding();
            await Clean();
        }

        public async UniTask RemoveBuilding(Vector3 destroyPosition)
        {
            if (IsEmpty)
                return;

            await TileRepresentation.DestroyBuilding(destroyPosition);
            await Clean();
        }

        protected virtual UniTask Clean()
        {
            SetBuilding(null);

            return default;
        }

        protected virtual async UniTask SetUpBuilding(Building building)
        {
            await CreateBuildingRepresentation(building);
        }

        protected virtual async UniTask CreateGroundRepresentation(bool isWaitedForCreation)
        {
            await TileRepresentation.GroundCreator.Create(Type);
        }

        protected virtual async UniTask CreateBuildingRepresentation(Building building)
        {
            SetBuilding(building);

            await Building.CreateRepresentation(TileRepresentation, true, true);
        }

        protected void SetBuilding(Building building)
        {
            Building = building;
            TileData.BuildingType = IsEmpty ? BuildingType.Undefined : Building.Type;
        }
    }
}
