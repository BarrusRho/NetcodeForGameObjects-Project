using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace NetcodeForGameObjects.Core
{
    public class HealthBarUI : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private Health _health;
        [SerializeField] private Image _healthBarUI;

        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                return;
            }

            _health.CurrentHealth.OnValueChanged += OnHealthChanged;
            OnHealthChanged(0, _health.CurrentHealth.Value);
        }

        public override void OnNetworkDespawn()
        {
            if (!IsClient)
            {
                return;
            }

            _health.CurrentHealth.OnValueChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int oldHealthValue, int newHealthValue)
        {
            _healthBarUI.fillAmount = (float)newHealthValue / _health.MaxHealth;
        }
    }
}
