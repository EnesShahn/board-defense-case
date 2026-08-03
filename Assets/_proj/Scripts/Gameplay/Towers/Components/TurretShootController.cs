using System;
using Game.Enemies.Components;
using Game.Towers.ConfigDatas;
using Game.Towers.Root;
using UnityEngine;

namespace Game.Towers.Components
{
    public class TurretShootController : MonoBehaviour, ITowerComponent
    {
        [SerializeField] private TowerRoot _towerRoot;
        [SerializeField] private TowerEnemyDetector _towerEnemyDetector;

        private TowerRoot _targetTowerRoot;
        private TowerFireRateConfigData _towerFireRateConfigData;
        private TowerDamageConfigData _towerDamageConfigData;

        private float _fireTimer;

        public TowerRoot TowerRoot => _towerRoot;

        public event Action KilledEnemy;

        private void Awake()
        {
            _towerFireRateConfigData = _towerRoot.TowerConfig.GetTowerData<TowerFireRateConfigData>();
            _towerDamageConfigData = _towerRoot.TowerConfig.GetTowerData<TowerDamageConfigData>();
        }
        private void Update()
        {
            if (!_towerEnemyDetector.AnyEnemyInRange)
                return;

            _fireTimer += Time.deltaTime * _towerFireRateConfigData.FireRate;
            if (_fireTimer >= 1)
            {
                _fireTimer = 0;

                var targetEnemy = _towerEnemyDetector.EnemiesInRange[0];

                var enemyHealthController = targetEnemy.GetComp<EnemyHealthController>();
                enemyHealthController.ReceiveDamage(_towerDamageConfigData.Damage);
                if (enemyHealthController.CurrentHealth == 0)
                    KilledEnemy?.Invoke();
            }
        }
    }
}