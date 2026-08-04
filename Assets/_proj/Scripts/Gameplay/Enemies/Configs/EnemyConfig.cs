using System;
using System.Collections.Generic;
using ESF.Core.PField;
using Game.Enemies.Root;
using UnityEngine;

namespace Game.Enemies.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private EnemyConfigId _enemyConfigId;
        [SerializeField] private EnemyRoot _prefab;
        [SerializeField] private PList<IEnemyData> _enemyData;

        [NonSerialized] private bool _enemyInitialized;
        private Dictionary<Type, IEnemyData> _enemyDataMap = new();

        public int EnemyConfigId => _enemyConfigId.Value;
        public EnemyRoot Prefab => _prefab;

        public T GetEnemyData<T>() where T : class, IEnemyData
        {
            if (!_enemyInitialized) // lazy init
            {
                _enemyInitialized = true;
                foreach (var towerData in _enemyData)
                {
                    _enemyDataMap.Add(towerData.GetType(), towerData);
                }
            }

            Type t = typeof(T);
            if (!_enemyDataMap.ContainsKey(t))
                throw new ArgumentException($"Type {t} data doesn't exist on this Tower Config ID {_enemyConfigId.Value}");

            return _enemyDataMap[t] as T;
        }
    }
}