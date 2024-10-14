namespace Assets.Sources.Gameplay.GameplayMover
{
    public interface IExpandingGameplayMover : IGameplayMover
    {
        void ExpandWorld(uint targetLength, uint targetWidth);
    }
}