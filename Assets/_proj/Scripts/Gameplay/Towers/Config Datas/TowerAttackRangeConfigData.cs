using System;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Towers.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, "ESF.Gameplay.Towers", "ESF.Gameplay", "TowerAttackRange")]
    public class TowerAttackRangeConfigData : ITowerData
    {
        [SerializeField] private float _range;

        public float Range => _range;
    }
}