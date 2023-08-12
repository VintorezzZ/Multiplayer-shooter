using UnityEngine;

namespace DefaultNamespace
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;

        private int _max;
        private int _current;

        public void SetMax(int max)
        {
            _max = max;
            UpdateHpView();
        }

        public void SetCurrent(int current)
        {
            _current = current;
            UpdateHpView();
        }

        public void ApplyDamage(int damage)
        {
            _current -= damage;
            UpdateHpView();
        }

        private void UpdateHpView()
        {
            _healthBarView.UpdateHealth(_max, _current);
        }
    }
}