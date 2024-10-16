﻿using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;

namespace Assets.Sources.Data
{
    public interface IExpandingWorldData : ICurrencyWorldData
    {
        event Action<BuildingType> BuildingUpdated;
    }
}