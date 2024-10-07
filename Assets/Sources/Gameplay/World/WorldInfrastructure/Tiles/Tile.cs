using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StaticDataService;
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

        public Tile(TileType type, Vector2Int greedPosition, IStaticDataService staticDataService, BuildingType buildingType)
        {
            Type = type;
            GridPosition = greedPosition;
            StaticDataService = staticDataService;
            BuildingType = buildingType;
        }

        public BuildingType BuildingType { get; protected set; }
        public bool IsEmpty => BuildingType == BuildingType.Undefined;

        protected TileRepresentation TileRepresentation { get; private set; }

        public async UniTask CreateRepresentation(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            TileRepresentation = await tileRepresentationCreatable.Create(GridPosition);
            await CreateGroundRepresentation();
            await TileRepresentation.TryChangeBuilding(BuildingType);
        }

        public virtual async UniTask PutBuilding(BuildingType buildingType)
        {
            BuildingType = buildingType;

            await TileRepresentation.TryChangeBuilding(BuildingType);
        }

        public virtual void RemoveBuilding()
        {
            BuildingType = BuildingType.Undefined;
            TileRepresentation.DestroyBuilding();
        }

        protected virtual async UniTask CreateGroundRepresentation()
        {
            await TileRepresentation.GroundCreator.Create(Type);
        }
    }
}
