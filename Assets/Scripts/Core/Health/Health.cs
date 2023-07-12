using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class Health : NetworkBehaviour
    {
        private bool _isDead;

        [field: SerializeField] public int MaxHealth { get; private set; } = 100;

        public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

        public Action<Health> OnDeath;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }

            CurrentHealth.Value = MaxHealth;
        }

        private void ModifyHealth(int value)
        {

        }

        public void TakeDamage(int damageAmount)
        {
            ModifyHealth(-damageAmount);
        }

        public void RestoreHealth(int healthAmount)
        {
            if (_isDead)
            {
                return;
            }

            var newHealthAmount = CurrentHealth.Value + healthAmount;
            CurrentHealth.Value = Mathf.Clamp(newHealthAmount, 0, MaxHealth);

            if (CurrentHealth.Value == 0)
            {
                OnDeath?.Invoke(this);
                _isDead = true;
            }
        }
    }
}
