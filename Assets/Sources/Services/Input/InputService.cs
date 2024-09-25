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

            _inputActionsSheme.GameplayInput.Enable();
            _inputActionsSheme.GameplayWindowInput.Enable();

            _inputActionsSheme.GameplayInput.HandlePressedMove.performed += OnHandlePressedMoveStarted;
            _inputActionsSheme.GameplayInput.HandlePressedMove.performed += OnHandlePressedMovePerformed;
            _inputActionsSheme.GameplayInput.HandlePressedMove.canceled += OnHandlePressedMoveCancled;
            _inputActionsSheme.GameplayInput.HandleMove.performed += OnHandleMovePerformed;

            _inputActionsSheme.GameplayWindowInput.UndoButtonPressed.performed += ctx => UndoButtonPressed?.Invoke();
            _inputActionsSheme.GameplayWindowInput.RemoveBuildingButtonPressed.performed += ctx => RemoveBuildingButtonPressed?.Invoke();
            _inputActionsSheme.GameplayWindowInput.ReplaceBuildingButtonPressed.performed += ctx => ReplaceBuildingButtonPressed?.Invoke();

        }

        ~InputService()
        {
            _inputActionsSheme.GameplayInput.HandlePressedMove.performed -= OnHandlePressedMoveStarted;
            _inputActionsSheme.GameplayInput.HandlePressedMove.performed -= OnHandlePressedMovePerformed;
            _inputActionsSheme.GameplayInput.HandlePressedMove.canceled -= OnHandlePressedMoveCancled;
            _inputActionsSheme.GameplayInput.HandleMove.performed -= OnHandleMovePerformed;

            _inputActionsSheme.Disable();
        }

        public event Action<Vector2> HandlePressedMoveStarted;
        public event Action<Vector2> HandlePressedMovePerformed;
        public event Action<Vector2> Pressed;
        public event Action<Vector2> HandleMoved;

        public event Action UndoButtonPressed;
        public event Action RemoveBuildingButtonPressed;
        public event Action ReplaceBuildingButtonPressed;

        private void OnHandleMovePerformed(InputAction.CallbackContext callbackContext)
        {
            _lastHandleMovePerformedPosition = callbackContext.ReadValue<Vector2>();
            HandleMoved?.Invoke(_lastHandleMovePerformedPosition);
        }

        private void OnHandlePressedMoveCancled(InputAction.CallbackContext obj) =>
            Pressed?.Invoke(_lastHandleMovePerformedPosition);

        private void OnHandlePressedMoveStarted(InputAction.CallbackContext obj) =>
            HandlePressedMoveStarted?.Invoke(_lastHandleMovePerformedPosition);

        private void OnHandlePressedMovePerformed(InputAction.CallbackContext obj) =>
            HandlePressedMovePerformed?.Invoke(_lastHandleMovePerformedPosition);
    }
}
