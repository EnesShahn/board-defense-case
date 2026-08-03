using System;
using Game.Towers.Components;
using Game.Towers.Root;
using UnityEngine;

namespace Game.Towers.Brains
{
    public class TurretTowerBrain : MonoBehaviour
    {
        [SerializeField] private TowerRoot _towerRoot;
        [SerializeField] private TowerHealthController _towerHealthController;
        [SerializeField] private TowerDestructionController _towerDestructionController;
        [SerializeField] private TowerEnemyDetector _towerEnemyDetector;

        private void Awake()
        {
            _towerRoot.TowerPreCreate.AddListener(OnTowerPreCreate);
        }
        private void OnTowerPreCreate(object sender, TowerRoot e)
        {
            _towerHealthController.ResetHealth();
            _towerDestructionController.ResetDestroyState();
            _towerEnemyDetector.ClearEnemies();
        }
    }
}