using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Enemy Config Collection")]
    public class EnemyConfigCollection : ScriptableObject
    {
        [SerializeField] private List<EnemyConfig> _enemyConfigs;

        public List<EnemyConfig> EnemyConfigs => _enemyConfigs;
    }
}