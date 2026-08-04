using System;
using Game.Enemies.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Enemies.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, sourceAssembly: "ESF.Gameplay")]
    public class EnemyHealthConfigData : IEnemyData
    {
        [SerializeField] private int _maxHealth;

        public int MaxHealth => _maxHealth;
    }
}