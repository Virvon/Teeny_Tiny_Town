﻿using System;
using Assets.Sources.Services.StaticDataService.Configs.Building;

namespace Assets.Sources.Services.StaticDataService.Configs.Quests
{
    [Serializable]
    public class QuestConfig
    {
        public string Id;
        public uint Reward;
        public string Info;
        public BuildingType BuildingType;
        public uint TargetCount;
    }
}