﻿using UnityEngine;

namespace DefaultNamespace
{
    public abstract class Character : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; protected set; } = 2f;
        public Vector3 Velocity { get; protected set; }
    }
}