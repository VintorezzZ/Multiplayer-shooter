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

        public void Init(Vector3 velocity)
        {
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
            Dispose();
        }

        private void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
