using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSystem
{
    public class LevelService
    {
        private readonly List<LevelConfig> _levelConfigs;

        private int _activeLevelIndex;
        private GameObject _activeLevel;

        public GameObject ActiveLevel => _activeLevel;
        public int ActiveLevelIndex => _activeLevelIndex;
        public List<LevelConfig> LevelConfigs => _levelConfigs;

        public LevelService(List<LevelConfig> levelConfigs)
        {
            _levelConfigs = levelConfigs;
        }

        public GameObject CreateLevel(int levelIndex)
        {
            if (levelIndex >= _levelConfigs.Count)
                return null;

            var newLevel = GameObject.Instantiate(_levelConfigs[levelIndex].Prefab, Vector3.zero, Quaternion.identity);

            _activeLevel = newLevel;
            _activeLevelIndex = levelIndex;
            return newLevel;
        }
        public void DestroyActiveLevel()
        {
            if (_activeLevel == null)
                return;

            GameObject.DestroyImmediate(_activeLevel);

            _activeLevel = null;
            _activeLevelIndex = -1;
        }
    }
}