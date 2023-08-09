using UnityEngine;

namespace DefaultNamespace
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _radius;
        [SerializeField] private float _coyoteTime = .15f;

        private float _flyTimer = 0f;

        public bool IsGrounded { get; private set; }

        private void Update()
        {
            if (Physics.CheckSphere(transform.position, _radius, _layerMask))
            {
                IsGrounded = true;
                _flyTimer = 0;
            }
            else
            {
                _flyTimer += Time.deltaTime;

                if (_flyTimer > _coyoteTime)
                    IsGrounded = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, _radius);
        }
#endif

    }
}