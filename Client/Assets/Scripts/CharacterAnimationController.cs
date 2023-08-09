using DefaultNamespace;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Speed = Animator.StringToHash("Speed");

    [SerializeField] private Animator _animator;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Character _character;

    private void Update()
    {
        var localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
        var speed = localVelocity.magnitude / _character.MaxSpeed;
        var sign = Mathf.Sign(localVelocity.z);

        _animator.SetFloat(Speed, speed * sign);
        _animator.SetBool(Grounded, _groundChecker.IsGrounded);
    }
}
