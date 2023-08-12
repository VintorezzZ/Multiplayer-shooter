using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private RectTransform _filledImage;
        [SerializeField] private float _defaultWidth;

        private void OnValidate()
        {
            _defaultWidth = _filledImage.sizeDelta.x;
        }

        public void UpdateHealth(float max, int current)
        {
            var fillPercent = current / max;
            _filledImage.sizeDelta = new Vector2(_defaultWidth * fillPercent, _filledImage.sizeDelta.y);
        }
    }
}