using System;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public interface IBuildingsUpdatable
    {
        event Action UpdateFinished;
    }
}
