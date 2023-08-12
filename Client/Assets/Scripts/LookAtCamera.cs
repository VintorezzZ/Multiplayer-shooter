﻿using UnityEngine;

namespace DefaultNamespace
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _camera;

        private void Start()
        {
            _camera = Camera.main.transform;
        }

        private void Update()
        {
            transform.LookAt(_camera);
        }
    }
}