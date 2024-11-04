﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Sources.Services.Input
{
    public class InputService : IInputService, IDisposable
    {
        private readonly InputActionsSheme _inputActionsSheme;

        private Vector2 _lastHandleMovePerformedPosition;

        public InputService()
        {
            _inputActionsSheme = new();

            //_inputActionsSheme.GameplayInput.Enable();
            //_inputActionsSheme.GameplayWindowInput.Enable();

            _inputActionsSheme.GameplayInput.HandlePressedMove.started += OnHandlePressedMoveStarted;
            _inputActionsSheme.GameplayInput.HandlePressedMove.performed += OnHandlePressedMovePerformed;
            _inputActionsSheme.GameplayInput.HandlePressedMove.canceled += OnHandlePressedMoveCancled;
            _inputActionsSheme.GameplayInput.HandleMove.performed += OnHandleMovePerformed;

            _inputActionsSheme.GameplayWindowInput.UndoButtonPressed.performed += ctx => UndoButtonPressed?.Invoke();
            _inputActionsSheme.GameplayWindowInput.RemoveBuildingButtonPressed.performed += ctx => RemoveBuildingButtonPressed?.Invoke();
            _inputActionsSheme.GameplayWindowInput.ReplaceBuildingButtonPressed.performed += ctx => ReplaceBuildingButtonPressed?.Invoke();

            _inputActionsSheme.SandboxWindowInput.ClearTilesButtonPressed.performed += ctx => ClearTilesButtonPressed?.Invoke();
            _inputActionsSheme.SandboxWindowInput.BuildingsButtonPressed.performed += ctx => BuildingsButtonPressed?.Invoke();
            _inputActionsSheme.SandboxWindowInput.GroundsButtonPressed.performed += ctx => GroundsButtonPressed?.Invoke();
        }

        public event Action<Vector2> HandlePressedMoveStarted;
        public event Action<Vector2> HandlePressedMovePerformed;
        public event Action<Vector2> Pressed;
        public event Action<Vector2> HandleMoved;

        public event Action UndoButtonPressed;
        public event Action RemoveBuildingButtonPressed;
        public event Action ReplaceBuildingButtonPressed;

        public event Action ClearTilesButtonPressed;
        public event Action BuildingsButtonPressed;
        public event Action GroundsButtonPressed;

        public void Dispose()
        {
            _inputActionsSheme.GameplayInput.HandlePressedMove.started -= OnHandlePressedMoveStarted;
            _inputActionsSheme.GameplayInput.HandlePressedMove.performed -= OnHandlePressedMovePerformed;
            _inputActionsSheme.GameplayInput.HandlePressedMove.canceled -= OnHandlePressedMoveCancled;
            _inputActionsSheme.GameplayInput.HandleMove.performed -= OnHandleMovePerformed;

            _inputActionsSheme.Disable();
        }

        public void SetEnabled(bool enabled)
        {
            if (enabled)
                _inputActionsSheme.Enable();
            else
                _inputActionsSheme.Disable();
        }

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
