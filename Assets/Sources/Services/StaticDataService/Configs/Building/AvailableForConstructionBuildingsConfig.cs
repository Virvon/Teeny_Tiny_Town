using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Building
{
    [CreateAssetMenu(fileName = "AvailableForConstructionBuildingsConfig", menuName = "StaticData/Create new available for construction buildings config", order = 51)]
    public class AvailableForConstructionBuildingsConfig : ScriptableObject
    {
        public uint requiredCreatedBuildingsToAddNext;
        public uint AvailableUpgradeBrach;
        
        public BuildingUpgradeBranch[] Branches;

        private void OnValidate()
        {
            if (AvailableUpgradeBrach >= Branches.Length)
            {
                Debug.LogError("Invalid value for " + nameof(AvailableUpgradeBrach));
                AvailableUpgradeBrach = 0;
            }
        }

        public bool TryFindeNextBuilding(BuildingType buildingType, out BuildingType nextBuildingType)
        {
            nextBuildingType = BuildingType.Undefined;

            foreach (BuildingUpgradeBranch branch in Branches)
            {
                foreach (BuildingUpgradeBranchElement branchElement in branch.Elements)
                {
                    if (branchElement.BuildingType == buildingType)
                    {
                        nextBuildingType = branchElement.NextBuildingType;
                        return true;
                    }
                }
            }

            return false;
        }
    }   
}