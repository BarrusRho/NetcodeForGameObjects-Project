using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;
using System;

namespace NetcodeForGameObjects.Input
{
    [CreateAssetMenu(fileName = "InputManager", menuName = "Input/InputManager")]
    public class InputManager : ScriptableObject, IPlayerActions
    {
        private Controls _controls;

        public event Action<Vector2> MoveEvent;
        public event Action<bool> PrimaryFireEvent;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPrimaryFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PrimaryFireEvent?.Invoke(true);
            }
            else if (context.canceled)
            {
                PrimaryFireEvent?.Invoke(false);
            }
        }
    }
}

