using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class WorldData
    {
        public List<TileData> Tiles;

        public WorldData(uint length, uint width, List<TileData> tiles)
        {
            Tiles = tiles;
        }
    }
}
