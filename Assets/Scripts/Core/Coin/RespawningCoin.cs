using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class RespawningCoin : CoinBase
    {
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

            return _coinValue;
        }
    }
}

