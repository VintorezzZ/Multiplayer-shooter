using System;
using System.Collections;
using System.Collections.Generic;
using Armory.Weapons;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    private static readonly int WeaponShoot = Animator.StringToHash("Shoot");

    [SerializeField] private Weapon _weapon;
    [SerializeField] private Animator _animator;

    void Start()
    {
        _weapon.WeaponShoot += Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger(WeaponShoot);
    }

    private void OnDestroy()
    {
        _weapon.WeaponShoot -= Shoot;
    }
}
