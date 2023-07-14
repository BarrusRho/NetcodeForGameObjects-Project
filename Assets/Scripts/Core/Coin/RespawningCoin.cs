using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class RespawningCoin : CoinBase
    {
        public event Action<RespawningCoin> OnCoinCollected;

        private Vector3 _initialPosition;

        private void Update()
        {
            if (_initialPosition != transform.position)
            {
                Show(true);
            }
            _initialPosition = transform.position;
        }

        public override int Collect()
        {
            if (!IsServer)
            {
                Show(false);
                return 0;
            }

            if (_isCollected)
            {
                return 0;
            }

            _isCollected = true;

            OnCoinCollected?.Invoke(this);

            return _coinValue;
        }

        public void ResetCoin()
        {
            _isCollected = false;
        }
    }
}

