using System;
using Armory.Projectiles;
using UnityEngine;

namespace Armory.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected Bullet _bulletPrefab;

        public Action ShootAction;
    }
}
