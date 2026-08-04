using System;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Towers.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, sourceAssembly: "ESF.Gameplay")]
    public class TowerDamageConfigData : ITowerData
    {
        [SerializeField] private int _damage;

        public int Damage => _damage;
    }
}