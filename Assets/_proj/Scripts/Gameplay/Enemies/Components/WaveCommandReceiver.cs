using System;
using Game.Enemies.Root;
using UnityEngine;

namespace Game.Enemies.Components
{
    public class WaveCommandReceiver : MonoBehaviour, IEnemyComponent
    {
        [SerializeField] private EnemyRoot _enemyRoot;

        public EnemyRoot EnemyRoot => _enemyRoot;

        public event Action<int> BeginAssaultCommandReceived; // int = lane index

        public void BeginAssault(int laneIndex)
        {
            BeginAssaultCommandReceived?.Invoke(laneIndex);
        }
    }
}