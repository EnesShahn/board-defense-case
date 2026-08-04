using System;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Towers.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, sourceAssembly: "ESF.Gameplay")]
    public class TowerHealthConfigData : ITowerData
    {
        [SerializeField] private int _maxHealth;

        public int MaxHealth => _maxHealth;
    }
}