using System;
using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const int MAX_RECEIVED_TIME_INTERVALS_COUNT = 5;

    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0f;

    private readonly Queue<float> _receivedTimeInterval = new Queue<float>();
    private float _lastReceivedTime = 0f;

    private void Start()
    {
        TargetPosition = transform.position;
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
        _velocityMagnitude = velocity.magnitude;
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
        var velocity = Vector3.zero;

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

                default: Debug.LogWarning("incorrect field " + dataChange.Field);
                    break;
            }
        }

        SetMovement(position, velocity, _receivedTimeInterval.Average());
    }
}
