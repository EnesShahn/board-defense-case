using System;
using UnityEngine;
using Game.Enemies.ConfigDatas;
using Game.Enemies.Root;
using Game.Towers.Components;
using Game.Towers.Root;

namespace Game.Enemies.Components
{
    public class BasicEnemyShootAtTowerController : MonoBehaviour, IEnemyComponent
    {
        [SerializeField] private EnemyRoot _enemyRoot;

        private TowerRoot _targetTowerRoot;
        private EnemyFireRateConfigData _enemyFireRateConfigData;
        private EnemyDamageConfigData _enemyDamageConfigData;

        private float _fireTimer;

        public EnemyRoot EnemyRoot => _enemyRoot;

        public event Action TargetTowerDestroyed;

        private void Awake()
        {
            _enemyFireRateConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyFireRateConfigData>();
            _enemyDamageConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyDamageConfigData>();
        }
        private void Update()
        {
            if (_targetTowerRoot == null)
                return;


            _fireTimer += Time.deltaTime * _enemyFireRateConfigData.FireRate;
            if (_fireTimer >= 1)
            {
                _fireTimer = 0;

                var towerHealthController = _targetTowerRoot.GetComp<TowerHealthController>();
                towerHealthController.ReceiveDamage(_enemyDamageConfigData.Damage);
            }
        }

        public void SetTargetTower(TowerRoot towerRoot)
        {
            _targetTowerRoot = towerRoot;
        }
    }
}