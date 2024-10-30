namespace Assets.Sources.Data.WorldDatas.Currency
{
    public interface ICurrencyWorldData : IWorldData
    {
        WorldWallet WorldWallet { get; }
        WorldMovesCounterData MovesCounter { get; }
        WorldStore WorldStore { get; }
    }
}