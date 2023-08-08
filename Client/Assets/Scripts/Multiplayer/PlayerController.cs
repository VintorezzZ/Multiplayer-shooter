using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _minHeadAngle = -90f;
    [SerializeField] private float _maxHeadAngle = 90f;
    [SerializeField] private float _jumpForce = 10f;

    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;
    private bool _isGrounded = true;

    private void Start()
    {
        var mainCamera = Camera.main.transform;
        mainCamera.parent = _cameraPoint;
        mainCamera.localPosition = Vector3.zero;
        mainCamera.localRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        Move();
        RotateY();
    }

    public void SetInput(float h, float v, float rotateY)
    {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
    }

    private void Move()
    {
        //var direction = new Vector3(_inputH, 0, _inputV).normalized;
        //transform.position += direction * _speed * Time.deltaTime;

        var velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
    }

    private void RotateY()
    {
        _rigidbody.angularVelocity = new Vector3(0f, _rotateY, 0f);
        _rotateY = 0;
    }

    public void RotateX(float value)
    {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
    }

    public void Jump()
    {
        if (!_isGrounded)
            return;

        _rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
    }


    private void OnCollisionStay(Collision collision)
    {
        var contactPoints = collision.contacts;

        foreach (var contactPoint in contactPoints)
        {
            if (contactPoint.normal.y > 0.45f)
                _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        _isGrounded = false;
    }
}