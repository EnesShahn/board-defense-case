using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemies.Configs;

namespace Game.WaveSystem
{
    [CreateAssetMenu(menuName = "Game/Wave Controller Config")]
    public class WaveControllerConfig : ScriptableObject
    {
        [SerializeField] private WaveConfig[] _waveConfigs;

        public WaveConfig[] WaveConfigs => _waveConfigs;
    }

    [Serializable]
    public class WaveConfig
    {
        [SerializeField] private float _waveInterval = 3;
        [SerializeField] private List<WaveEnemyConfig> _waveEnemyConfigs;

        public float WaveInterval => _waveInterval;
        public List<WaveEnemyConfig> WaveEnemyConfigs => _waveEnemyConfigs;
    }

    [Serializable]
    public class WaveEnemyConfig
    {
        [SerializeField] private EnemyConfigId _enemyConfigId;
        [SerializeField] private float _spawnInterval = 0.5f;
        [SerializeField] private int _spawnCount;

        public EnemyConfigId EnemyConfigId => _enemyConfigId;
        public float SpawnInterval => _spawnInterval;
        public int SpawnCount => _spawnCount;
    }
}