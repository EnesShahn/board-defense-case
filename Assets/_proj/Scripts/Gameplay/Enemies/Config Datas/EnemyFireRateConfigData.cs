using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Game.Enemies.Configs;

namespace Game.Enemies.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, "ESF.Gameplay.Enemies", "ESF.Gameplay", "EnemyFireRate")]
    public class EnemyFireRateConfigData : IEnemyData
    {
        [SerializeField] private float _fireRate;

        public float FireRate => _fireRate;
    }
}