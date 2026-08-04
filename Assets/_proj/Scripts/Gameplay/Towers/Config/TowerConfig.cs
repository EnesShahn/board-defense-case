using System;
using System.Collections.Generic;
using UnityEngine;
using ESF.Core.PField;
using Game.Towers.Root;

namespace Game.Towers.Configs
{
    [CreateAssetMenu(menuName = "Game/Towers/Tower Config")]
    public class TowerConfig : ScriptableObject
    {
        [SerializeField] private TowerConfigId _towerConfigId;
        [SerializeField] private string _towerName;
        [SerializeField] private Sprite _towerIcon;
        [SerializeField] private TowerRoot _prefab;
        [SerializeField] private PList<ITowerData> _towerData;

        [NonSerialized] private bool _towerInitialized;
        private Dictionary<Type, ITowerData> _towerDataMap = new();

        public int TowerConfigId => _towerConfigId.Value;
        public string TowerName => _towerName;
        public Sprite TowerIcon => _towerIcon;
        public TowerRoot Prefab => _prefab;

        public T GetTowerData<T>() where T : class, ITowerData
        {
            if (!_towerInitialized) // lazy init
            {
                _towerInitialized = true;
                foreach (var towerData in _towerData)
                {
                    _towerDataMap.Add(towerData.GetType(), towerData);
                }
            }

            Type t = typeof(T);
            if (!_towerDataMap.ContainsKey(t))
                throw new ArgumentException($"Type {t} data doesn't exist on this Tower Config ID {_towerConfigId.Value}");

            return _towerDataMap[t] as T;
        }
    }
}