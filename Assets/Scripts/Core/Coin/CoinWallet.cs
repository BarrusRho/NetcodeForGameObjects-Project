using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class CoinWallet : NetworkBehaviour
    {
        public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<CoinBase>(out var coin))
            {
                return;
            }

            var coinValue = coin.Collect();

            if (!IsServer)
            {
                return;
            }

            TotalCoins.Value += coinValue;
        }
    }
}
