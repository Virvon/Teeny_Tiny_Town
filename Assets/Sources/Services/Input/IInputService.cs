using System;
using UnityEngine;

namespace Assets.Sources.Services.Input
{
    public interface IInputService
    {
        event Action<Vector2> HandleMoved;
        event Action<Vector2> HandlePressedMovePerformed;
        event Action<Vector2> HandlePressedMoveStarted;
        event Action<Vector2> Pressed;
        event Action UndoButtonPressed;
        event Action RemoveBuildingButtonPressed;
        event Action ReplaceBuildingButtonPressed;
    }
}