using System;
using System.Collections;
using System.Collections.Generic;
using NetcodeForGameObjects.Input;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class PlayerShooting : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private CoinWallet _coinWallet;
        [SerializeField] private Collider2D _playerCollider;
        [SerializeField] private GameObject _serverProjectilePrefab;
        [SerializeField] private GameObject _clientProjectilePrefab;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private GameObject _muzzleFlash;

        [Header("Settings")]
        [SerializeField] private float _fireRate;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _muzzleFlashDuration;
        [SerializeField] private int _costToFire = 1;

        private bool _canShoot;
        private float _gameTimer;
        private float _muzzleFlashTimer;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }

            _inputManager.PrimaryFireEvent += OnPrimaryFire;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
            {
                return;
            }

            _inputManager.PrimaryFireEvent -= OnPrimaryFire;
        }

        private void Update()
        {
            if (_muzzleFlashTimer > 0f)
            {
                _muzzleFlashTimer -= Time.deltaTime;
                if (_muzzleFlashTimer <= 0f)
                {
                    _muzzleFlash.SetActive(false);
                }
            }

            if (!IsOwner)
            {
                return;
            }

            if (_gameTimer > 0f)
            {
                _gameTimer -= Time.deltaTime;
            }

            if (!_canShoot)
            {
                return;
            }

            if (_gameTimer > 0f)
            {
                return;
            }

            if (_coinWallet.TotalCoins.Value < _costToFire)
            {
                return;
            }

            PrimaryFireServerRpc(_projectileSpawnPoint.position, _projectileSpawnPoint.up);

            SpawnDummyProjectile(_projectileSpawnPoint.position, _projectileSpawnPoint.up);

            _gameTimer = 1 / _fireRate;
        }

        private void OnPrimaryFire(bool canShoot)
        {
            _canShoot = canShoot;
        }

        [ServerRpc]
        private void PrimaryFireServerRpc(Vector3 spawnPosition, Vector3 projectileDirection)
        {
            if (_coinWallet.TotalCoins.Value < _costToFire)
            {
                return;
            }

            _coinWallet.SpendCoins(_costToFire);

            var projectile = Instantiate(_serverProjectilePrefab, spawnPosition, Quaternion.identity);
            projectile.transform.up = projectileDirection;

            var projectileCollider = projectile.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(_playerCollider, projectileCollider);

            if (projectile.TryGetComponent<DamageOnContact>(out var damageOnContactComponent))
            {
                damageOnContactComponent.SetOwner(OwnerClientId);
            }

            if (projectile.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.velocity = rigidbody2D.transform.up * _projectileSpeed;
            }

            SpawnDummyProjectileClientRpc(spawnPosition, projectileDirection);
        }

        [ClientRpc]
        private void SpawnDummyProjectileClientRpc(Vector3 spawnPosition, Vector3 projectileDirection)
        {
            if (IsOwner)
            {
                return;
            }

            SpawnDummyProjectile(spawnPosition, projectileDirection);
        }

        private void SpawnDummyProjectile(Vector3 spawnPosition, Vector3 projectileDirection)
        {

            _muzzleFlash.SetActive(true);
            _muzzleFlashTimer = _muzzleFlashDuration;

            var projectile = Instantiate(_clientProjectilePrefab, spawnPosition, Quaternion.identity);
            projectile.transform.up = projectileDirection;

            var projectileCollider = projectile.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(_playerCollider, projectileCollider);

            if (projectile.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.velocity = rigidbody2D.transform.up * _projectileSpeed;
            }
        }
    }
}
