using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public interface IExpandingGameplayMover : ICurrencyGameplayMover
    {
        void ExpandWorld(Vector2Int targetSize);
    }
}