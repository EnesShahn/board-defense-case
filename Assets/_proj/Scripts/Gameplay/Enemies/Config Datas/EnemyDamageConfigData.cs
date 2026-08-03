using System;
using Game.Enemies.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Enemies.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, "ESF.Gameplay.Enemies", "ESF.Gameplay", "EnemyDamage")]
    public class EnemyDamageConfigData : IEnemyData
    {
        [SerializeField] private int _damage;

        public int Damage => _damage;
    }
}