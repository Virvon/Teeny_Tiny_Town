using Assets.Sources.Services.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ActionHandlerStateMachine
    {
        private readonly IInputService _inputService;
        public readonly Dictionary<Type, ActionHandlerState> _states;

        private bool _isActive;

        public ActionHandlerStateMachine(IInputService inputService)
        {
            _inputService = inputService;

            _states = new();
            _isActive = false;

            _inputService.HandleMoved += OnHandleMoved;
            _inputService.Pressed += OnPressed;
            _inputService.HandlePressedMoveStarted += OnHandlePressedMoveStarted;
            _inputService.HandlePressedMovePerformed += OnHandlePressedMovePerformed;
        }

        ~ActionHandlerStateMachine()
        {
            _inputService.HandleMoved -= OnHandleMoved;
            _inputService.Pressed -= OnPressed;
            _inputService.HandlePressedMoveStarted -= OnHandlePressedMoveStarted;
            _inputService.HandlePressedMovePerformed -= OnHandlePressedMovePerformed;
        }

        public ActionHandlerState CurrentState { get; private set; }

        public void Enter<TState>() where TState : ActionHandlerState
        {
            CurrentState?.Exit();
            CurrentState = _states[typeof(TState)];
            CurrentState?.Enter();
        }

        public void RegisterState<TState>(TState handlerState) where TState : ActionHandlerState =>
            _states.Add(typeof(TState), handlerState);

        public void SetActive(bool value)
        {
            _isActive = value;
        }

        private void OnHandleMoved(Vector2 handlePosition)
        {
            if (CurrentState != null && _isActive)
                CurrentState.OnHandleMoved(handlePosition);
        }

        private void OnPressed(Vector2 handlePosition)
        {
            if (CurrentState != null && _isActive)
                CurrentState.OnPressed(handlePosition);
        }

        private void OnHandlePressedMovePerformed(Vector2 handlePosition)
        {
            if (CurrentState != null && _isActive)
                CurrentState.OnHandlePressedMovePerformed(handlePosition);
        }

        private void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            if (CurrentState != null && _isActive)
                CurrentState.OnHandlePressedMoveStarted(handlePosition);
        }
    }
}
