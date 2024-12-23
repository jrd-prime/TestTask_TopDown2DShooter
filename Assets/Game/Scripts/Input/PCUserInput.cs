using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Input
{
    public interface IUserInput
    {
        public ReactiveProperty<Vector2> MoveDirection { get; }
        public ReactiveProperty<Vector2> MousePosition { get; }
    }

    public sealed class PCUserInput : MonoBehaviour, IUserInput
    {
        public ReactiveProperty<Vector2> MoveDirection { get; } = new();
        public ReactiveProperty<Vector2> MousePosition { get; } = new();

        private PCInputActions _gameInputActions;
        private Vector2 _movementInput;
        private Camera _cam;


        private void Awake()
        {
            InputSystem.EnableDevice(Mouse.current);
            _cam = Camera.main;
            Debug.LogWarning("Enable PCInputActions");
            _gameInputActions = new PCInputActions();
            _gameInputActions.Enable();

            _gameInputActions.Player.Move.performed += OnMovePerformed;
            _gameInputActions.Player.Move.canceled += OnMoveCanceled;

            _gameInputActions.Player.MousePosition.performed += OnMousePositionPerformed;
        }

        private void OnMousePositionPerformed(InputAction.CallbackContext context)
        {
            MousePosition.Value = _cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
            // MoveDirection.Value = context.ReadValue<Vector2>();
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            MoveDirection.Value = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            if (MoveDirection.CurrentValue != Vector2.zero) MoveDirection.Value = Vector2.zero;
        }

        private void OnDestroy()
        {
            _gameInputActions.Player.Move.performed -= OnMovePerformed;
            _gameInputActions.Player.Move.canceled -= OnMoveCanceled;
            _gameInputActions.Player.MousePosition.performed -= OnMousePositionPerformed;
            _gameInputActions.Disable();
        }
    }
}
