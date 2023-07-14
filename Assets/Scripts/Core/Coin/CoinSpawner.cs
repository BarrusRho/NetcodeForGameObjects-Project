using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class CoinSpawner : NetworkBehaviour
    {
        [SerializeField] private RespawningCoin _coinPrefab;
        [SerializeField] private int _maxCoins = 50;
        [SerializeField] private int _coinValue = 10;
        [SerializeField] private Vector2 _xSpawnRange;
        [SerializeField] private Vector2 _ySpawnRange;

        [SerializeField] private LayerMask _spawnLayerMask;

        private float _coinRadius;
        private Collider2D[] _coinBufferArray = new Collider2D[1];

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }

            _coinRadius = _coinPrefab.GetComponent<CircleCollider2D>().radius;

            for (int i = 0; i < _maxCoins; i++)
            {
                SpawnCoin();
            }
        }

        private void SpawnCoin()
        {
            var coin = Instantiate(_coinPrefab, GetSpawnPoint(), Quaternion.identity);
            coin.SetValue(_coinValue);
            coin.GetComponent<NetworkObject>().Spawn();
            coin.OnCoinCollected += OnCoinCollected;
        }

        private void OnCoinCollected(RespawningCoin coin)
        {
            coin.transform.position = GetSpawnPoint();
            coin.ResetCoin();
        }

        private Vector2 GetSpawnPoint()
        {
            float x = 0;
            float y = 0;

            while (true)
            {
                x = Random.Range(_xSpawnRange.x, _xSpawnRange.y);
                y = Random.Range(_ySpawnRange.x, _ySpawnRange.y);
                var spawnPoint = new Vector2(x, y);

                var colliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, _coinRadius, _coinBufferArray, _spawnLayerMask);
                if (colliders == 0)
                {
                    return spawnPoint;
                }
            }
        }
    }
}
