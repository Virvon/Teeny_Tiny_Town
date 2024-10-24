﻿using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings
{
    public class Lighthouse : Building
    {
        private readonly WorldWallet _worldWallet;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ITileGetable _tileGetable;
        private readonly Vector2Int _gridPosition;
        private readonly List<Tile> _aroundTiles;


        public Lighthouse(BuildingType type, WorldWallet worldWallet, IPersistentProgressService persistentProgressService, ITileGetable tileGetable, Vector2Int gridPosition)
            : base(type)
        {
            _worldWallet = worldWallet;
            _persistentProgressService = persistentProgressService;
            _tileGetable = tileGetable;
            _gridPosition = gridPosition;

            _aroundTiles = GetAroundTiles();

            _persistentProgressService.Progress.MoveCounter.TimeToPaymentPayableBuildings += OnTimeToPaymentPayableBuildings;
        }

        ~Lighthouse() =>
            _persistentProgressService.Progress.MoveCounter.TimeToPaymentPayableBuildings -= OnTimeToPaymentPayableBuildings;

        public override UniTask CreateRepresentation(TileRepresentation tileRepresentation, bool waitForCompletion)
        {
            

            return base.CreateRepresentation(tileRepresentation, waitForCompletion);
        }

        private void OnTimeToPaymentPayableBuildings()
        {
            uint payment = 0;

            foreach(Tile tile in _aroundTiles)
            {
                if (tile.Building is PayableBuilding payableBuilding)
                    payment += payableBuilding.Payment;
            }

            _worldWallet.Give(payment);
        }

        private List<Tile> GetAroundTiles()
        {
            List<Tile> tiles = new();

            foreach (int positionY in _tileGetable.GetLineNeighbors(_gridPosition.y))
            {
                foreach (int positionX in _tileGetable.GetLineNeighbors(_gridPosition.x))
                {
                    Tile tile = _tileGetable.GetTile(new Vector2Int(positionX, positionY));

                    if (tile != null)
                        tiles.Add(tile);
                }
            }

            return tiles;
        }
    }
}
