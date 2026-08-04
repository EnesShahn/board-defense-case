using System.Collections.Generic;
using ESF.Core.Logging;
using Game.Towers.Root;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Towers.Configs
{
    public class TowerConfigService
    {
        private readonly List<TowerConfig> _towerConfigs;
        private readonly Dictionary<int, TowerConfig> _towerConfigMap = new();
        private readonly Dictionary<int, ObjectPool<TowerRoot>> _towerPrefabPoolMap = new();

        private List<TowerRoot> _activeTowers = new();

        private Transform _towerPoolParent;

        public TowerConfigService(List<TowerConfig> towerConfigs)
        {
            _towerConfigs = towerConfigs;
            _towerPoolParent = new GameObject("Towers Pool").transform;
        }

        public void InitializeConfigsAndPools()
        {
            // Pre disable all gameobject in advance to prevent race conditions with Awake generally
            foreach (var towerConfig in _towerConfigs)
            {
                if (towerConfig.Prefab == null)
                {
                    ELogger.LogError<TowerConfigService>("an active Tower Config has Null prefab");
                    continue;
                }

                towerConfig.Prefab.gameObject.SetActive(false);
            }

            foreach (var towerConfig in _towerConfigs)
            {
                var newPool = new ObjectPool<TowerRoot>(createFunc: () =>
                {
                    var newTower = Object.Instantiate(towerConfig.Prefab, _towerPoolParent);

                    newTower.SetTowerConfig(towerConfig);
                    newTower.TowerPrePoolAllocate?.Invoke(this, newTower);
                    newTower.gameObject.SetActive(false);
                    newTower.TowerPostPoolAllocate?.Invoke(this, newTower);

                    return newTower;
                });

                _towerConfigMap.Add(towerConfig.TowerConfigId, towerConfig);
                _towerPrefabPoolMap.Add(towerConfig.TowerConfigId, newPool);
            }
        }

        public void Dispose()
        {
            foreach (var prefabPool in _towerPrefabPoolMap)
                prefabPool.Value.Dispose();
            foreach (var activeTower in _activeTowers)
                if (activeTower != null)
                    GameObject.DestroyImmediate(activeTower.gameObject);
            if (_towerPoolParent != null)
                GameObject.DestroyImmediate(_towerPoolParent.gameObject);
        }


        public TowerConfig GetTowerConfig(int towerConfigId)
        {
            if (!_towerConfigMap.ContainsKey(towerConfigId))
                return null;

            return _towerConfigMap[towerConfigId];
        }

        public TowerRoot CreateTower(int towerConfigId)
        {
            if (!_towerConfigMap.ContainsKey(towerConfigId))
                return null;

            var newTower = _towerPrefabPoolMap[towerConfigId].Get();
            _activeTowers.Add(newTower);

            newTower.TowerPreCreate?.Invoke(this, newTower);
            newTower.transform.SetParent(null);
            newTower.SetIsCreatedAndActive(true);
            newTower.TowerPostCreate?.Invoke(this, newTower);

            return newTower;
        }
        public bool DestroyTower(TowerRoot towerRoot)
        {
            if (towerRoot == null || towerRoot.TowerConfig == null || towerRoot.TowerConfig.TowerConfigId == -1)
                return false;

            var towerConfigId = towerRoot.TowerConfig.TowerConfigId;

            towerRoot.TowerPreDestroy?.Invoke(this, towerRoot);
            towerRoot.transform.SetParent(_towerPoolParent);
            towerRoot.gameObject.SetActive(false);
            towerRoot.SetIsCreatedAndActive(false);
            _towerPrefabPoolMap[towerConfigId].Release(towerRoot);

            towerRoot.TowerPostDestroy?.Invoke(this, towerRoot);

            _activeTowers.Remove(towerRoot);

            return true;
        }
    }
}