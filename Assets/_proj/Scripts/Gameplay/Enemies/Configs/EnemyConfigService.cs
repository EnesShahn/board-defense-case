using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ESF.Core.Logging;
using Game.Enemies.Root;

namespace Game.Enemies.Configs
{
    public class EnemyConfigService
    {
        private readonly List<EnemyConfig> _enemyConfigs;
        private readonly Dictionary<int, EnemyConfig> _enemyConfigMap = new();
        private readonly Dictionary<int, ObjectPool<EnemyRoot>> _enemyPrefabPoolMap = new();

        private Transform _enemyPoolParent;

        public EnemyConfigService(List<EnemyConfig> enemyConfigs)
        {
            _enemyConfigs = enemyConfigs;
            _enemyPoolParent = new GameObject("Enemy Pool").transform;
        }

        public void InitializeConfigsAndPools()
        {
            // Pre disable all gameobject in advance to prevent race conditions with Awake generally
            foreach (var enemyConfig in _enemyConfigs)
            {
                if (enemyConfig.Prefab == null)
                {
                    ELogger.LogError<EnemyConfigService>("an active Enemy Config has Null prefab");
                    continue;
                }

                enemyConfig.Prefab.gameObject.SetActive(false);
            }

            foreach (var enemyConfig in _enemyConfigs)
            {
                var newPool = new ObjectPool<EnemyRoot>(createFunc: () =>
                {
                    var newEnemy = Object.Instantiate(enemyConfig.Prefab, _enemyPoolParent);

                    newEnemy.SetEnemyConfig(enemyConfig);
                    newEnemy.EnemyPrePoolAllocate?.Invoke(this, newEnemy);
                    newEnemy.gameObject.SetActive(false);
                    newEnemy.EnemyPostPoolAllocate?.Invoke(this, newEnemy);

                    return newEnemy;
                });

                _enemyConfigMap.Add(enemyConfig.EnemyConfigId, enemyConfig);
                _enemyPrefabPoolMap.Add(enemyConfig.EnemyConfigId, newPool);
            }
        }

        public void Dispose()
        {
            foreach (var prefabPool in _enemyPrefabPoolMap)
            {
                prefabPool.Value.Dispose();
            }

            GameObject.DestroyImmediate(_enemyPoolParent);
        }


        public EnemyRoot CreateEnemy(int enemyConfigId)
        {
            if (!_enemyConfigMap.ContainsKey(enemyConfigId))
                return null;

            var newEnemy = _enemyPrefabPoolMap[enemyConfigId].Get();
            newEnemy.EnemyPreCreate?.Invoke(this, newEnemy);

            newEnemy.transform.SetParent(null);

            newEnemy.EnemyPostCreate?.Invoke(this, newEnemy);

            return newEnemy;
        }
        public void DestroyEnemy(EnemyRoot enemyRoot)
        {
            if (enemyRoot == null || enemyRoot.EnemyConfig == null || enemyRoot.EnemyConfig.EnemyConfigId == -1)
                return;

            var enemyConfigId = enemyRoot.EnemyConfig.EnemyConfigId;

            enemyRoot.EnemyPreDestroy?.Invoke(this, enemyRoot);

            enemyRoot.transform.SetParent(_enemyPoolParent);
            enemyRoot.gameObject.SetActive(false);
            _enemyPrefabPoolMap[enemyConfigId].Release(enemyRoot);

            enemyRoot.EnemyPostDestroy?.Invoke(this, enemyRoot);
        }
    }
}