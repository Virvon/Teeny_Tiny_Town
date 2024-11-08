using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Sources.Services.Input
{
    public class InputService : IInputService, IDisposable
    {
        private readonly InputActionsSheme _inputActionsSheme;

        private Vector2 _lastHandleMovePerformedPosition;
        private float _previousMagnitude;

        public InputService()
        {
            _inputActionsSheme = new ();

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

            _inputActionsSheme.SandboxInput.MouseScroll.performed += OnMouseZoomed;
            TouchscreenZoom();

            _inputActionsSheme.SandboxInput.RotateWorld.performed += ctx => Rotated?.Invoke(ctx.ReadValue<Vector2>().x);
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

        public event Action<float> Zoomed;
        public event Action<float> Rotated;

        public void Dispose()
        {
            _inputActionsSheme.GameplayInput.HandlePressedMove.started -= OnHandlePressedMoveStarted;
            _inputActionsSheme.GameplayInput.HandlePressedMove.performed -= OnHandlePressedMovePerformed;
            _inputActionsSheme.GameplayInput.HandlePressedMove.canceled -= OnHandlePressedMoveCancled;
            _inputActionsSheme.GameplayInput.HandleMove.performed -= OnHandleMovePerformed;
            _inputActionsSheme.SandboxInput.MouseScroll.performed -= OnMouseZoomed;

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

        private void OnMouseZoomed(InputAction.CallbackContext ctx)
        {
            float zoomValue = Mathf.Clamp(ctx.ReadValue<Vector2>().y, -1, 1);

            Zoomed?.Invoke(zoomValue);
        }

        private void TouchscreenZoom()
        {
            _inputActionsSheme.SandboxInput.SecondTouch.performed += _ =>
            {
                float magnitude = _inputActionsSheme.SandboxInput.FirstTouch.ReadValue<Vector2>().y - _inputActionsSheme.SandboxInput.SecondTouch.ReadValue<Vector2>().y;

                if (_previousMagnitude == 0)
                    _previousMagnitude = magnitude;

                float difference = magnitude - _previousMagnitude;

                _previousMagnitude = magnitude;

                float zoomValue = Mathf.Clamp(difference, -1, 1);

                Zoomed?.Invoke(zoomValue);
            };

            _inputActionsSheme.SandboxInput.SecondTouch.canceled += _ =>
            {
                _previousMagnitude = 0;
            };
        }
    }
}
