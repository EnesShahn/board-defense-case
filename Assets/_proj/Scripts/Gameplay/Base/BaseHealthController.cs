using System;
using UnityEngine;
using ESF.UI;

namespace Game.Bases
{
    public class BaseHealthController : MonoBehaviour
    {
        [SerializeField] private BaseRoot _baseRoot;
        [SerializeField] private UIBar _hpBar;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _animationSpeed = 10f;
        [SerializeField] private float _minAnimationSpeed = 2f;

        private float _currentHealth;
        private float _currentVisualHealth;

        public BaseRoot BaseRoot => _baseRoot;
        public int MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        public event Action HealthReachedZero;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _currentVisualHealth = _maxHealth;
            
            UpdateUI();
        }
        private void Update()
        {
            float diff = Mathf.Max(_minAnimationSpeed, Mathf.Abs(_currentVisualHealth - _currentHealth));
            _currentVisualHealth = Mathf.MoveTowards(_currentVisualHealth, _currentHealth, _animationSpeed * diff * Time.deltaTime);
            UpdateUI();
        }

        public void ReceiveDamage(float damageAmount)
        {
            if (_currentHealth == 0)
                return;

            _currentHealth -= damageAmount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            if (_currentHealth <= 0)
            {
                HealthReachedZero?.Invoke();
            }

            UpdateUI();
        }

        public void UpdateUI()
        {
            _hpBar.SetFill(_currentVisualHealth / _maxHealth);
        }
    }
}