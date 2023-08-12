using UnityEngine;

namespace Armory.Weapons
{
    public class ClientWeapon : Weapon
    {
        [SerializeField] private Transform _bulletPoint;
        [SerializeField] private float _bulletSpeed = 10f;
        [SerializeField] private float _shootDelay = 0.2f;
        [SerializeField] private int _damage = 10;

        private float _lastShootTime;


        public bool TryShoot(out ShootInfo info)
        {
            info = new ShootInfo();

            if (Time.time - _lastShootTime < _shootDelay)
                return false;

            var position = _bulletPoint.position;
            var velocity = _bulletPoint.forward * _bulletSpeed;

            _lastShootTime = Time.time;
            var bullet = Instantiate(_bulletPrefab, position, _bulletPoint.rotation);
            bullet.Init(velocity, _damage);
            ShootAction?.Invoke();

            info.PosX = position.x;
            info.PosY = position.y;
            info.PosZ = position.z;

            info.DirX = velocity.x;
            info.DirY = velocity.y;
            info.DirZ = velocity.z;

            return true;
        }
    }
}