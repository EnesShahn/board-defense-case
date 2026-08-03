using System;
using UnityEngine;
using Game.Bases;
using Game.Enemies.ConfigDatas;
using Game.Enemies.Root;

namespace Game.Enemies.Components
{
    public class BasicEnemyShootAtBaseController : MonoBehaviour, IEnemyComponent
    {
        [SerializeField] private EnemyRoot _enemyRoot;

        private BaseRoot _targetBaseRoot;
        private EnemyFireRateConfigData _enemyFireRateConfigData;
        private EnemyDamageConfigData _enemyDamageConfigData;

        private float _fireTimer;

        public EnemyRoot EnemyRoot => _enemyRoot;

        public event Action TargetBaseDestroyed;

        private void Awake()
        {
            _enemyFireRateConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyFireRateConfigData>();
            _enemyDamageConfigData = _enemyRoot.EnemyConfig.GetEnemyData<EnemyDamageConfigData>();
        }
        private void Update()
        {
            if (_targetBaseRoot == null)
                return;
            
            _fireTimer += Time.deltaTime * _enemyFireRateConfigData.FireRate;
            if (_fireTimer >= 1)
            {
                _fireTimer = 0;

                var baseHealthController = _targetBaseRoot.GetComponentInChildren<BaseHealthController>();
                baseHealthController.ReceiveDamage(_enemyDamageConfigData.Damage);
            }
        }

        public void SetTargetBase(BaseRoot baseRoot)
        {
            _targetBaseRoot = baseRoot;
        }
    }
}