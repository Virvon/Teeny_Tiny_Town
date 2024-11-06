using Assets.Sources.Data;
using Assets.Sources.Data.World;
using Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "StaticData/WorldConfig/Create new world config", order = 51)]
    public class WorldConfig : ScriptableObject
    {
        public string Id;
        public Vector2Int Size;
        public TileConfig[] TileConfigs;
        public BuildingType NextBuildingTypeForCreation;
        public BuildingType[] StartingAvailableBuildingTypes;
        public AssetReferenceGameObject AssetReference;
        public RewardType[] AvailableRewards;
        public int MinRewardVariantsCount;
        public int MaxRewardVariantsCount;
        public AdditionalBonusType[] AvailableAdditionalBonuses;
        public bool IsUnlockedOnStart;
        public uint Cost;
        public string Name;
        public AssetReference[] PeculiarityIconAssetReferences;
        public string NextWorldId;
        public string PreviousWorldId;

        public TileType GetTileType(Vector2Int gridPosition) =>
            TileConfigs.First(tile => tile.GridPosition == gridPosition).Type;

        public TileData[] TilesDatas => TileConfigs.Select(tileConfig => new TileData(tileConfig.GridPosition, tileConfig.BuildingType)).ToArray();

        private void OnValidate() =>
            CreateTileConfigs();

        public virtual WorldData GetWorldData(uint[] goals, IStaticDataService staticDataService) => 
            new WorldData(Id, TilesDatas, NextBuildingTypeForCreation, StartingAvailableBuildingTypes.ToList(), Size, goals, IsUnlockedOnStart);

        private void CreateTileConfigs()
        {
            TileConfig[] newTileConfigs = new TileConfig[Size.x * Size.y];

            if(TileConfigs == null)
            {
                int i = 0;

                for (int x = 0; x < Size.x; x++)
                {
                    for (int z = 0; z < Size.y; z++)
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

                for (int x = 0; x < Size.x; x++)
                {
                    for (int z = 0; z < Size.y; z++)
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
