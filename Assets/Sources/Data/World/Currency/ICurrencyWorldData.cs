namespace Assets.Sources.Data.World.Currency
{
    public interface ICurrencyWorldData : IWorldData
    {
        WorldWallet WorldWallet { get; }
        WorldMovesCounterData MovesCounter { get; }
        WorldStore WorldStore { get; }
    }
}