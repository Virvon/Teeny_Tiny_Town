using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Sources.Services.Input
{
    public class InputService : IInputService
    {
        private readonly InputActionsSheme _inputActionsSheme;

        private Vector2 _lastHandleMovePerformedPosition;

        public InputService()
        {
            _inputActionsSheme = new();
            _inputActionsSheme.Enable();

            _inputActionsSheme.Input.HandlePressedMove.performed += OnHandlePressedMoveStarted;
            _inputActionsSheme.Input.HandlePressedMove.performed += OnHandlePressedMovePerformed;
            _inputActionsSheme.Input.HandlePressedMove.canceled += OnHandlePressedMoveCancled;
            _inputActionsSheme.Input.HandleMove.performed += OnHandleMovePerformed;
        }

        ~InputService()
        {
            _inputActionsSheme.Input.HandlePressedMove.performed -= OnHandlePressedMoveStarted;
            _inputActionsSheme.Input.HandlePressedMove.performed -= OnHandlePressedMovePerformed;
            _inputActionsSheme.Input.HandlePressedMove.canceled -= OnHandlePressedMoveCancled;
            _inputActionsSheme.Input.HandleMove.performed -= OnHandleMovePerformed;

            _inputActionsSheme.Disable();
        }

        public event Action HandlePressedMoveStarted;
        public event Action HandlePressedMovePerformed;
        public event Action<Vector2> Pressed;
        public event Action<Vector2> HandleMoved;


        private void OnHandleMovePerformed(InputAction.CallbackContext callbackContext)
        {
            _lastHandleMovePerformedPosition = callbackContext.ReadValue<Vector2>();
            HandleMoved?.Invoke(_lastHandleMovePerformedPosition);
        }

        private void OnHandlePressedMoveCancled(InputAction.CallbackContext obj) =>
            Pressed?.Invoke(_lastHandleMovePerformedPosition);

        private void OnHandlePressedMoveStarted(InputAction.CallbackContext obj) =>
            HandlePressedMoveStarted?.Invoke();

        private void OnHandlePressedMovePerformed(InputAction.CallbackContext obj) =>
            HandlePressedMovePerformed?.Invoke();
    }
}
