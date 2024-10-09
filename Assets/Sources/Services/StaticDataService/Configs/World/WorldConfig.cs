using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [Serializable]
    public class WorldConfig
    {
        public uint Length;
        public uint Width;
        public TileConfig[] TileConfigs;
        public BuildingType NextBuildingTypeForCreation;

        public void CreateTileConfigs()
        {
            TileConfig[] newTileConfigs = new TileConfig[Length * Width];

            if(TileConfigs == null)
            {
                int i = 0;

                for (int x = 0; x < Length; x++)
                {
                    for (int z = 0; z < Width; z++)
                    {
                        newTileConfigs[i] = new TileConfig(new Vector2Int(x, z));
                        i++;
                    }
                }

                TileConfigs = newTileConfigs;
            }
            else
            {
                int i = 0;

                for (int x = 0; x < Length; x++)
                {
                    for (int z = 0; z < Width; z++)
                    {
                        if(i >= TileConfigs.Length)
                        {
                            newTileConfigs[i] = new TileConfig(new Vector2Int(x, z));
                        }
                        else
                        {
                            newTileConfigs[i] = TileConfigs[i];
                        }

                        i++;
                    }
                }

                TileConfigs = newTileConfigs;
            }
        }
    }
}
