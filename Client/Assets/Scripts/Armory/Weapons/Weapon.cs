using System;
using Armory.Projectiles;
using UnityEngine;

namespace Armory.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletPoint;
        [SerializeField] private float _bulletSpeed = 10f;
        [SerializeField] private float _shootDelay = 0.2f;

        private float _lastShootTime;

        public Action WeaponShoot;

        public void Shoot()
        {
            if (Time.time - _lastShootTime < _shootDelay)
                return;

            _lastShootTime = Time.time;

            var bullet = Instantiate(_bulletPrefab, _bulletPoint.position, _bulletPoint.rotation);
            bullet.Init(_bulletPoint.forward, _bulletSpeed);

            WeaponShoot?.Invoke();
        }
    }
}
