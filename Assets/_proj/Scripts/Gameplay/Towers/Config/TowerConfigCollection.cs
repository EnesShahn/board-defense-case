using System.Collections.Generic;
using UnityEngine;

namespace Game.Towers.Configs
{
    [CreateAssetMenu(menuName = "Game/Towers/Tower Config Collection")]
    public class TowerConfigCollection : ScriptableObject
    {
        [SerializeField] private List<TowerConfig> _towerConfigs;

        public List<TowerConfig> TowerConfigs => _towerConfigs;
    }
}