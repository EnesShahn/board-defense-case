using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ESF.Core.Services;
using ESF.Utilities.Extensions;
using Game.Cells;
using Game.Enemies.Components;
using Game.Enemies.Configs;
using Game.Enemies.Root;

namespace Game.WaveSystem
{
    public class WaveController : MonoBehaviour
    {
        [SerializeField] private CellManager _cellManager;
        [SerializeField] private WaveControllerConfig _waveControllerConfig;
        [SerializeField] private Transform[] _enemyStartPositionPerLane;

        private List<HashSet<EnemyRoot>> _spawnedEnemiesByLane = new();
        private EnemyConfigService _enemyConfigService;

        private bool _isWaveActive;

        private int _currentWaveIndex;
        private float _currentWaveInterval;
        private float _waveTimer;

        private int _currentWaveEnemy;
        private int _currentWaveEnemySpawnCount;
        private float _spawnTimer;

        public bool IsWaveActive => _isWaveActive;
        public int CurrentWaveIndex => _currentWaveIndex;
        public float CurrentWaveInterval => _currentWaveInterval;
        public float WaveTimer => _waveTimer;

        public event Action<int> WaveStarted; // int = Wave Index
        public event Action<int> WaveCompleted; // int = Wave Index
        public event Action AllWavesCompleted;

        private void Awake()
        {
            _enemyConfigService = Service.Resolve<EnemyConfigService>();

            for (int i = 0; i < _cellManager.GetLaneCount(); i++)
                _spawnedEnemiesByLane.Add(new());
        }

        private void Update()
        {
            if (_currentWaveIndex >= _waveControllerConfig.WaveConfigs.Length)
                return;

            if (_isWaveActive)
            {
                var currentWave = _waveControllerConfig.WaveConfigs[_currentWaveIndex];
                var currentWaveEnemyConfigs = currentWave.WaveEnemyConfigs;
                var currentWaveEnemyConfig = currentWaveEnemyConfigs[_currentWaveEnemy];

                // already spawned count, wait for them to finish
                if (_currentWaveEnemySpawnCount >= currentWaveEnemyConfig.SpawnCount)
                {
                    // if (GetTotalActiveEnemies() == 0)
                    {
                        _currentWaveIndex++;
                        if (_currentWaveIndex >= _waveControllerConfig.WaveConfigs.Length)
                            _currentWaveInterval = 0;
                        else
                            _currentWaveInterval = _waveControllerConfig.WaveConfigs[_currentWaveIndex].WaveInterval;
                        _spawnTimer = 0;
                        _waveTimer = 0;
                        _isWaveActive = false;
                        WaveCompleted?.Invoke(_currentWaveIndex - 1);
                    }

                    return;
                }


                _spawnTimer += Time.deltaTime;
                if (_spawnTimer > currentWaveEnemyConfig.SpawnInterval)
                {
                    _spawnTimer = 0;

                    int laneIndex = GetRandomLeastOccupiedLaneIndex();
                    var enemyInstance = _enemyConfigService.CreateEnemy(currentWaveEnemyConfig.EnemyConfigId);
                    enemyInstance.gameObject.SetActive(true);
                    enemyInstance.transform.position = _enemyStartPositionPerLane[laneIndex].transform.position;
                    enemyInstance.GetComp<WaveCommandReceiver>().BeginAssault(laneIndex);
                    var enemyHealthController = enemyInstance.GetComp<EnemyHealthController>();
                    enemyHealthController.HealthReachedZero -= OnEnemyHealthReachedZero;
                    enemyHealthController.HealthReachedZero += OnEnemyHealthReachedZero;

                    _spawnedEnemiesByLane[laneIndex].Add(enemyInstance);
                    _currentWaveEnemySpawnCount++;
                }
            }
            else
            {
                if (_currentWaveIndex == _waveControllerConfig.WaveConfigs.Length)
                {
                    AllWavesCompleted?.Invoke();
                    enabled = false;
                    return;
                }

                var currentWave = _waveControllerConfig.WaveConfigs[_currentWaveIndex];
                var currentWaveInterval = currentWave.WaveInterval;

                _waveTimer += Time.deltaTime;

                if (_waveTimer > currentWaveInterval)
                {
                    _waveTimer = 0;
                    _isWaveActive = true;
                    _currentWaveEnemySpawnCount = 0;
                    WaveStarted?.Invoke(_currentWaveIndex);
                }
            }
        }

        private void OnEnemyHealthReachedZero(EnemyHealthController enemyHealthController)
        {
            enemyHealthController.HealthReachedZero -= OnEnemyHealthReachedZero;
            for (int i = 0; i < _cellManager.GetLaneCount(); i++)
            {
                _spawnedEnemiesByLane[i].Remove(enemyHealthController.EnemyRoot);
            }
        }

        private int GetTotalActiveEnemies()
        {
            int enemyCount = 0;
            foreach (var enemiesInLane in _spawnedEnemiesByLane)
            {
                enemyCount += enemiesInLane.Count;
            }

            return enemyCount;
        }

        private int GetRandomLeastOccupiedLaneIndex()
        {
            int lowestActiveEnemyCountInLane = int.MaxValue;
            foreach (var laneEnemies in _spawnedEnemiesByLane)
                if (lowestActiveEnemyCountInLane > laneEnemies.Count)
                    lowestActiveEnemyCountInLane = laneEnemies.Count;

            HashSet<int> leastOccupiedLanes = new HashSet<int>();
            for (int i = 0; i < _spawnedEnemiesByLane.Count; i++)
                if (_spawnedEnemiesByLane[i].Count <= lowestActiveEnemyCountInLane)
                    leastOccupiedLanes.Add(i);

            return leastOccupiedLanes.ToList().GetRandom();
        }
    }
}