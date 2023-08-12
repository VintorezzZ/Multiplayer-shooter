using System;
using System.Collections.Generic;
using Colyseus.Schema;
using DefaultNamespace;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _minHeadAngle = -90f;
    [SerializeField] private float _maxHeadAngle = 90f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _jumpDelay = .2f;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private HealthController _healthController;

    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;
    private float _lastJumpTime;

    private void Start()
    {
        var mainCamera = Camera.main.transform;
        mainCamera.parent = _cameraPoint;
        mainCamera.localPosition = Vector3.zero;
        mainCamera.localRotation = Quaternion.identity;

        _healthController.SetMax(MaxHealth);
        _healthController.SetCurrent(MaxHealth);
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
        var velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * Speed;
        velocity.y = _rigidbody.velocity.y;
        Velocity = velocity;
        _rigidbody.velocity = Velocity;
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
        if (!_groundChecker.IsGrounded)
            return;

        if (Time.time - _lastJumpTime < _jumpDelay)
            return;

        _lastJumpTime = Time.time;
        _rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;

        rotateY = transform.eulerAngles.y;
        rotateX = _head.localEulerAngles.x;
    }

    public void OnChange(List<DataChange> changes)
    {
        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "currHp": _healthController.SetCurrent((sbyte)dataChange.Value); break;

                // default: Debug.LogWarning("incorrect field " + dataChange.Field);
                //     break;
            }
        }
    }
}