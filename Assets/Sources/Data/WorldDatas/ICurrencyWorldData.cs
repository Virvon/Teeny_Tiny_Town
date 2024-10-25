using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;

namespace Assets.Sources.Data.WorldDatas
{
    public interface ICurrencyWorldData : IWorldData
    {
        event Action<BuildingType> StoreListUpdated;

        IReadOnlyList<BuildingType> StoreList { get; }
        WorldWallet WorldWallet { get; }
        WorldMovesCounterData MovesCounter { get; }
    }
}