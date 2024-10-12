using Assets.Sources.Data;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "StaticData/WorldConfig/Create new world config", order = 51)]
    public class WorldConfig : ScriptableObject
    {
        public string Id;
        public uint Length;
        public uint Width;
        public TileConfig[] TileConfigs;
        public BuildingType NextBuildingTypeForCreation;
        public List<BuildingType> StartStoreList;
        public BuildingType[] StartingAvailableBuildingTypes;
        public AssetReferenceGameObject AssetReference;

        protected List<TileData> TilesDatas => TileConfigs.Select(tileConfig => new TileData(tileConfig.GridPosition, tileConfig.BuildingType, tileConfig.Type)).ToList();

        private void OnValidate() =>
            CreateTileConfigs();

        public virtual WorldData GetWorldData() => 
            new WorldData(Id, TilesDatas, NextBuildingTypeForCreation, StartingAvailableBuildingTypes.ToList(), Length, Width, StartStoreList);

        private void CreateTileConfigs()
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
