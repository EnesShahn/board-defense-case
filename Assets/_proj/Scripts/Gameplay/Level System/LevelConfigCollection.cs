using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSystem
{
    [CreateAssetMenu(menuName = "Game/Level System/Level Config Collection")]
    public class LevelConfigCollection : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levelConfigs;

        public List<LevelConfig> LevelConfigs => _levelConfigs;
    }
}