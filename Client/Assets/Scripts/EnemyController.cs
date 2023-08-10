using System;
using System.Collections.Generic;
using System.Linq;
using Armory.Weapons;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : Character
{
    private const int MAX_RECEIVED_TIME_INTERVALS_COUNT = 5;

    [SerializeField] private Transform _head;
    [SerializeField] private EntityWeapon _weapon;

    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0f;

    private readonly Queue<float> _receivedTimeInterval = new Queue<float>();
    private float _lastReceivedTime = 0f;
    private Player _player;

    private void Start()
    {
        TargetPosition = transform.position;
    }

    public void Init(Player player)
    {
        _player = player;
        SetSpeed(_player.speed);

        player.OnChange += OnChange;
    }

    private void Update()
    {
        if (_velocityMagnitude > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, _velocityMagnitude * Time.deltaTime);
        }
        else
        {
            transform.position = TargetPosition;
        }
    }

    // in - передача по ссылке без возможности модификаций

    private void SetMovement(in Vector3 position, in Vector3 velocity, float averageInterval)
    {
        TargetPosition = position + (velocity * averageInterval);
        Velocity = velocity;
        _velocityMagnitude = Velocity.magnitude;
    }

    public void SetSpeed(float value)
    {
        Speed = value;
    }

    public void SetRotateX(float value)
    {
        _head.localEulerAngles = new Vector3(value, 0, 0);
    }

    public void SetRotateY(float value)
    {
        transform.localEulerAngles = new Vector3(0, value, 0);
    }

    public void Shoot(in ShootInfo shootInfo)
    {
        var position = new Vector3(shootInfo.PosX, shootInfo.PosY, shootInfo.PosZ);
        var velocity = new Vector3(shootInfo.DirX, shootInfo.DirY, shootInfo.DirZ);

        _weapon.Shoot(position, velocity);
    }

    private void UpdateReceivedTime()
    {
        var interval = Time.time - _lastReceivedTime;
        _lastReceivedTime = Time.time;

        _receivedTimeInterval.Enqueue(interval);

        if (_receivedTimeInterval.Count > MAX_RECEIVED_TIME_INTERVALS_COUNT)
            _receivedTimeInterval.Dequeue();
    }

    public void OnChange(List<DataChange> changes)
    {
        UpdateReceivedTime();

        var position = TargetPosition;
        var velocity = Velocity;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "pX": position.x = (float) dataChange.Value; break;
                case "pY": position.y = (float) dataChange.Value; break;
                case "pZ": position.z = (float) dataChange.Value; break;

                case "vX": velocity.x = (float) dataChange.Value; break;
                case "vY": velocity.y = (float) dataChange.Value; break;
                case "vZ": velocity.z = (float) dataChange.Value; break;

                case "rX": SetRotateX((float) dataChange.Value); break;
                case "rY": SetRotateY((float) dataChange.Value); break;

                default: Debug.LogWarning("incorrect field " + dataChange.Field);
                    break;
            }
        }

        SetMovement(position, velocity, _receivedTimeInterval.Average());
    }

    public void Dispose()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }
}
