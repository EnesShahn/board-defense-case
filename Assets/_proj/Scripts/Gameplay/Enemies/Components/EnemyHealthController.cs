using System;
using UnityEngine;
using ESF.UI;
using Game.Enemies.ConfigDatas;
using Game.Enemies.Root;

namespace Game.Enemies.Components
{
    public class EnemyHealthController : MonoBehaviour, IEnemyComponent
    {
        [SerializeField] private EnemyRoot _enemyRoot;
        [SerializeField] private UIBar _hpBar;
        [SerializeField] private float _animationSpeed = 10f;
        [SerializeField] private float _minAnimationSpeed = 2f;

        private EnemyHealthConfigData _enemyHealthConfigData;
        private int _maxHealth;
        private float _currentHealth;
        private float _currentVisualHealth;

        public EnemyRoot EnemyRoot => _enemyRoot;
        public int MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        public event Action<EnemyHealthController> HealthReachedZero;

        private void Awake()
        {
            _enemyHealthConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyHealthConfigData>();

            _maxHealth = _enemyHealthConfigData.MaxHealth;
            _currentHealth = _enemyHealthConfigData.MaxHealth;
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
                HealthReachedZero?.Invoke(this);
            }

            UpdateUI();
        }

        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
            _currentVisualHealth = _currentHealth;

            UpdateUI();
        }

        private void UpdateUI()
        {
            _hpBar.SetFill(_currentVisualHealth / _maxHealth);
        }
    }
}