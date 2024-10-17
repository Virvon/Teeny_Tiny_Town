namespace Assets.Sources.Gameplay.GameplayMover
{
    public interface IExpandingGameplayMover : ICurrencyGameplayMover
    {
        void ExpandWorld(uint targetLength, uint targetWidth);
    }
}