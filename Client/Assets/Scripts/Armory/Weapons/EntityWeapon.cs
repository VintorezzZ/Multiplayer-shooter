using UnityEngine;

namespace Armory.Weapons
{
    public class EntityWeapon : Weapon
    {
        public void Shoot (Vector3 position, Vector3 velocity)
        {
            var bullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
            bullet.Init(velocity);

            ShootAction?.Invoke();
        }
    }
}