using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Data;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Data.WorldDatas.Currency;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public class ExpandingWorldChanger : CurrencyWorldChanger, IExpandingWorldChanger
    {
        private bool _isExpanded;

        public ExpandingWorldChanger(
            IStaticDataService staticDataService,
            ICurrencyWorldData worldData,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
            : base(staticDataService, worldData, nextBuildingForPlacingCreator)
        {
            _isExpanded = false;
        }

        public async UniTask Expand()
        {
            _isExpanded = true;

            Clean();
            await Fill(TileRepresentationCreatable);
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
