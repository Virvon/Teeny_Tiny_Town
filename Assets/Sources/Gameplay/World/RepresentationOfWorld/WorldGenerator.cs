﻿using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldGenerator : MonoBehaviour, ITileRepresentationCreatable
    {
        [SerializeField] private TileRepresentation _tile;
        [SerializeField] private float _cellSize;

        private IWorldFactory _worldFactory;

        private List<TileRepresentation> _tiles;

        [Inject]
        private void Construct(IWorldFactory worldFactory)
        {
            _worldFactory = worldFactory;

            _tiles = new();
        }

        public void TestInspect(WorldChanger worldChanger)
        {
            WorldChanger = worldChanger;
        }

        public WorldChanger WorldChanger;

        public TileRepresentation GetTile(Vector2Int gridPosition) =>
            _tiles.First(tile => tile.GridPosition == gridPosition);

        public async UniTask<TileRepresentation> Create(Vector2Int gridPosition)
        {
            Vector3 worldPosition = GridToWorldPosition(gridPosition) + transform.position;
            TileRepresentation tileRepresentation = await _worldFactory.CreateTileRepresentation(worldPosition, transform);

            tileRepresentation.Init(gridPosition);

            _tiles.Add(tileRepresentation);

            return tileRepresentation;
        }

        private Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector3(
                gridPosition.x * _cellSize,
                transform.position.y,
                gridPosition.y * _cellSize);
        }

        public class Factory : PlaceholderFactory<string, UniTask<WorldGenerator>>
        {
        }
    }
}
