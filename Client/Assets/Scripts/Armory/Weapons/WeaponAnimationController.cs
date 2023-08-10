using UnityEngine;

namespace Armory.Weapons
{
    public class WeaponAnimationController : MonoBehaviour
    {
        private static readonly int WeaponShoot = Animator.StringToHash("Shoot");

        [SerializeField] private Weapon _weapon;
        [SerializeField] private Animator _animator;

        void Start()
        {
            _weapon.ShootAction += Shoot;
        }

        private void Shoot()
        {
            _animator.SetTrigger(WeaponShoot);
        }

        private void OnDestroy()
        {
            _weapon.ShootAction -= Shoot;
        }
    }
}
