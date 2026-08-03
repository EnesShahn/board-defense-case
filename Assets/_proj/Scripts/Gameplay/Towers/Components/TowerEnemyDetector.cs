using System;
using System.Collections.Generic;
using UnityEngine;
using ESF.Core.Physics;
using ESF.Core.Tags;
using Game.Enemies.Root;

namespace Game.Towers.Components
{
    public class TowerEnemyDetector : MonoBehaviour
    {
        [SerializeField] private TriggerEventsListener _triggerEventsListener;

        private readonly List<EnemyRoot> _enemiesInRange = new();
        private readonly HashSet<EnemyRoot> _enemiesInRangeHashSet = new();

        public List<EnemyRoot> EnemiesInRange => _enemiesInRange;
        public bool AnyEnemyInRange => _enemiesInRange.Count > 0;

        public event EventHandler<EnemyRoot> EnemyEntered;
        public event EventHandler<EnemyRoot> EnemyExited;

        private void Awake()
        {
            _triggerEventsListener.OnTriggerEntered.AddListener(OnTriggerEntered);
            _triggerEventsListener.OnTriggerExited.AddListener(OnTriggerExited);
        }

        private void OnTriggerEntered(object sender, Collider e)
        {
            if (!e.gameObject.TryGetTag<EnemyTag>(out var enemyTag))
                return;

            _enemiesInRange.Add(enemyTag.EnemyRoot);
            _enemiesInRangeHashSet.Add(enemyTag.EnemyRoot);
            EnemyEntered?.Invoke(this, enemyTag.EnemyRoot);
        }
        private void OnTriggerExited(object sender, Collider e)
        {
            if (!e.gameObject.TryGetTag<EnemyTag>(out var enemyTag))
                return;

            _enemiesInRange.Remove(enemyTag.EnemyRoot);
            _enemiesInRangeHashSet.Remove(enemyTag.EnemyRoot);
            EnemyExited?.Invoke(this, enemyTag.EnemyRoot);
        }

        public void ClearEnemies()
        {
            _enemiesInRange.Clear();
            _enemiesInRangeHashSet.Clear();
        }
    }
}