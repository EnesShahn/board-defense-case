using System;
using Game.Enemies.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Enemies.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, sourceAssembly: "ESF.Gameplay")]
    public class EnemyMoveSpeedConfigData : IEnemyData
    {
        [SerializeField] private float _moveSpeed;

        public float MoveSpeed => _moveSpeed;
    }
}