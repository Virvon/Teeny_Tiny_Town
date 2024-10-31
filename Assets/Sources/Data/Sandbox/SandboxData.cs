using System;
using UnityEngine;

namespace Assets.Sources.Data.Sandbox
{
    [Serializable]
    public class SandboxData
    {
        public SandboxTileData[] Tiles;

        public SandboxData(Vector2Int size)
        {
            CreateTiles(size);
        }

        private void CreateTiles(Vector2Int size)
        {
            Tiles = new SandboxTileData[size.x * size.y];

            int i = 0;

            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.y; z++)
                {
                    Tiles[i] = new SandboxTileData(new Vector2Int(x, z));
                    i++;
                }
            }
        }
    }
}
