using System;
using System.Collections;
using UnityEngine;

namespace Armory.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] private Rigidbody _rigidbody;
        private int _damage;

        public void Init(Vector3 velocity, int damage = 0)
        {
            _damage = damage;
            _rigidbody.velocity = velocity;
            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSecondsRealtime(_lifeTime);
            Dispose();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.ApplyDamage(_damage);
            }
            
            Dispose();
        }

        private void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
