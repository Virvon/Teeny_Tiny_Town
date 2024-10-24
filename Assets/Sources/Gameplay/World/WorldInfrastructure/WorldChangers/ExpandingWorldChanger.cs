﻿using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Data;
using Cysharp.Threading.Tasks;
using Assets.Sources.Services.PersistentProgress;
using System.Collections.Generic;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public class ExpandingWorldChanger : CurrencyWorldChanger, IExpandingWorldChanger
    {
        private bool _isExpanded;

        public ExpandingWorldChanger(
            IStaticDataService staticDataService,
            ICurrencyWorldData worldData,
            IPersistentProgressService persistentProgressService,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
            : base(staticDataService, worldData, persistentProgressService, nextBuildingForPlacingCreator)
        {
            _isExpanded = false;
        }

        public async UniTask Expand()
        {
            _isExpanded = true;
            Clean();
            await Fill(TileRepresentationCreatable);
            NextBuildingForPlacingCreator.MoveToNextBuilding(Tiles);
            Start();

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
