using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public interface ICenterChangeable
    {
        event Action<Vector2Int, bool> CenterChanged;
    }
}
