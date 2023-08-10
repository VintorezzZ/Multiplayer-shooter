using UnityEngine;

namespace Armory.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public void Init(Vector3 direction, float speed)
        {
            _rigidbody.velocity = direction * speed;
        }
    }
}
