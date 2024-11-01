using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Data;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Data.World.Currency;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Services.PersistentProgress;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public class ExpandingWorldChanger : CurrencyWorldChanger, IExpandingWorldChanger
    {
        private bool _isExpanded;
        private ITileRepresentationCreatable _tileRepresentationCreatable;

        public ExpandingWorldChanger(
            IStaticDataService staticDataService,
            ICurrencyWorldData worldData,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService)
            : base(staticDataService, worldData, nextBuildingForPlacingCreator, persistentProgressService)
        {
            _isExpanded = false;
        }

        public override UniTask Generate(ITileRepresentationCreatable tileRepresentationCreatable)
        {
            _tileRepresentationCreatable = tileRepresentationCreatable;
            return base.Generate(tileRepresentationCreatable);
        }

        public async UniTask Expand()
        {
            _isExpanded = true;

            Clean();
            await Fill(_tileRepresentationCreatable);
            OnCenterChanged(true);

            _isExpanded = false;
        }

        protected override List<Tile> CreateTiles(List<RoadTile> roadTiles, List<TallTile> tallTiles)
        {
            if (_isExpanded == false)
                return base.CreateTiles(roadTiles, tallTiles);

            List<Tile> tiles = new();

            foreach (TileData tileData in WorldData.Tiles)
            {
                if (IsTileFitsIntoGrid(tileData.GridPosition) == false)
                    continue;

                Tile tile = GetGround(roadTiles, tallTiles, tileData);

                tiles.Add(tile);
            }

            return tiles;
        }
    }
}
