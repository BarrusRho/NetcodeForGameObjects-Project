using System;
using System.Collections;
using System.Collections.Generic;
using NetcodeForGameObjects.Input;
using UnityEngine;

namespace NetcodeForGameObjects.Testing
{
    public class InputManagerTest : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;

        private void OnEnable()
        {
            _inputManager.MoveEvent += OnMove;
            _inputManager.PrimaryFireEvent += OnPrimaryFire;
        }

        private void OnDisable()
        {
            _inputManager.MoveEvent -= OnMove;
            _inputManager.PrimaryFireEvent -= OnPrimaryFire;
        }

        private void OnMove(Vector2 vector)
        {
            Debug.Log($"Movement detected");
        }


        private void OnPrimaryFire(bool obj)
        {
            Debug.Log($"Primary fire detected");
        }
    }
}

