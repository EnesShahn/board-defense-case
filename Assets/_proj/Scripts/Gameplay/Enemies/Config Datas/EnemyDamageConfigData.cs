using System;
using Game.Enemies.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Enemies.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, sourceAssembly: "ESF.Gameplay")]
    public class EnemyDamageConfigData : IEnemyData
    {
        [SerializeField] private int _damage;

        public int Damage => _damage;
    }
}