using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class WorldData
    {
        public List<TileData> Tiles;

        public WorldData(uint length, uint width)
        {
            Tiles = new();

            Create(length, width);
        }

        private void Create(uint length, uint width)
        {
            for (int x = 0; x < length; x++)
            {
                for (int z = 0; z < width; z++)
                    Tiles.Add(new TileData(new Vector2Int(x, z)));
            }
        }
    }
}
