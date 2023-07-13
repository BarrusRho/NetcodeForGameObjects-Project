using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class DamageOnContact : MonoBehaviour
    {
        [SerializeField] private int _damageAmount = 5;

        private ulong _ownerClientId;

        public void SetOwner(ulong ownerClientId)
        {
            _ownerClientId = ownerClientId;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody == null)
            {
                return;
            }

            if (other.attachedRigidbody.TryGetComponent<NetworkObject>(out var networkObject))
            {
                if (_ownerClientId == networkObject.OwnerClientId)
                {
                    return;
                }
            }

            if (other.attachedRigidbody.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(_damageAmount);
            }
        }
    }
}
