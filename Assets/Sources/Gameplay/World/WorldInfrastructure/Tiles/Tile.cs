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

        public virtual async UniTask PutBuilding(Building building)
        {
            if (building == null)
                return;

            Building = building;

            await Building.CreateRepresentation(TileRepresentation);
        }

        public void Clean()
        {
            if (IsEmpty)
                return;

            Building.Destroy(TileRepresentation);
            Building = null;
        }

        public virtual void RemoveBuilding() =>
            Clean();

        protected virtual async UniTask CreateGroundRepresentation()
        {
            await TileRepresentation.GroundCreator.Create(Type);
        }
    }
}
