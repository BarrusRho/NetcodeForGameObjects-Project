using System;
using System.Collections;
using System.Collections.Generic;
using NetcodeForGameObjects.Input;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _treadsTransform;

        [Header("Settings")]
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 30f;

        private Vector2 _previousMovementInput;


        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }

            _inputManager.MoveEvent += OnMove;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
            {
                return;
            }

            _inputManager.MoveEvent -= OnMove;
        }

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            var zRotation = _previousMovementInput.x * -_rotationSpeed * Time.deltaTime;
            _treadsTransform.Rotate(0f, 0f, zRotation);
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }

            _rigidbody2D.velocity = (Vector2)_treadsTransform.up * _previousMovementInput.y * _movementSpeed;
        }

        private void OnMove(Vector2 movementInput)
        {
            _previousMovementInput = movementInput;
        }
    }
}
