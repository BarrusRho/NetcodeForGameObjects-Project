using System.Collections;
using System.Collections.Generic;
using NetcodeForGameObjects.Input;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class PlayerAiming : NetworkBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private Transform _turretTransform;

        private void LateUpdate()
        {
            if (!IsOwner)
            {
                return;
            }

            var aimScreenPosition = _inputManager.AimInput;
            Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(aimScreenPosition);

            _turretTransform.up = new Vector2(aimWorldPosition.x - _turretTransform.position.x, aimWorldPosition.y - _turretTransform.position.y);
        }
    }
}
