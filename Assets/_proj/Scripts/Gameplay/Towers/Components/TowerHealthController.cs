using System;
using UnityEngine;
using ESF.UI;
using Game.Towers.ConfigDatas;
using Game.Towers.Root;

namespace Game.Towers.Components
{
    public class TowerHealthController : MonoBehaviour, ITowerComponent
    {
        [SerializeField] private TowerRoot _towerRoot;
        [SerializeField] private UIBar _hpBar;
        [SerializeField] private float _animationSpeed = 10f;
        [SerializeField] private float _minAnimationSpeed = 2f;

        private TowerHealthConfigData _towerHealthConfigData;
        private int _maxHealth;
        private float _currentHealth;
        private float _currentVisualHealth;

        public TowerRoot TowerRoot => _towerRoot;
        public int MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        public event Action<TowerHealthController> HealthReachedZero;

        private void Awake()
        {
            _towerHealthConfigData = _towerRoot.TowerConfig.GetTowerData<TowerHealthConfigData>();

            _maxHealth = _towerHealthConfigData.MaxHealth;
            _currentHealth = _towerHealthConfigData.MaxHealth;
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