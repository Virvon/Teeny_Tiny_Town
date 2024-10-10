using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings
{
    public class Building
    {
        public readonly BuildingType Type;

        public Building(BuildingType type)
        {
            Type = type;
        }

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
