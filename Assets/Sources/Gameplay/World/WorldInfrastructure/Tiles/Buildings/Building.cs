using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings
{
    public class Building
    {
        public Building(BuildingType type) =>
            Type = type;

        public BuildingType Type { get; protected set; }

        public virtual async UniTask CreateRepresentation(TileRepresentation tileRepresentation)
        {
            await tileRepresentation.TryChangeBuilding<BuildingRepresentation>(Type);
        }

        public void Destroy(TileRepresentation tileRepresentation)
        {
            tileRepresentation.DestroyBuilding();
        }
    }
}
