using System;
using UnityEngine;

namespace Assets.Sources.Services.Input
{
    public interface IInputService
    {
        event Action<Vector2> HandleMoved;
        event Action HandlePressedMovePerformed;
        event Action HandlePressedMoveStarted;
        event Action<Vector2> Pressed;
    }
}