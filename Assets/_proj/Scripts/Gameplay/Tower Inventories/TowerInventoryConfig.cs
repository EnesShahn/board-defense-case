using System;
using UnityEngine;
using Game.Towers.Configs;

namespace Game.TowerInventories
{
    [CreateAssetMenu(menuName = "ESF/Tower Inventory Config")]
    public class TowerInventoryConfig : ScriptableObject
    {
        [SerializeField] private TowerInventory[] _towerInventories;

        public TowerInventory[] TowerInventories => _towerInventories;
    }

    [Serializable]
    public class TowerInventory
    {
        [SerializeField] private TowerConfigId _towerConfigId;
        [SerializeField] private int _towerCount;

        public TowerConfigId TowerConfigId => _towerConfigId;
        public int TowerCount => _towerCount;
    }
}