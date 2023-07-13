using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public abstract class CoinBase : NetworkBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        protected int _coinValue = 10;
        protected bool _isCollected;

        public abstract int Collect();

        public void SetValue(int value)
        {
            _coinValue = value;
        }

        protected void Show(bool show)
        {
            _spriteRenderer.enabled = show;
        }
    }
}
