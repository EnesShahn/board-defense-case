using System;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Towers.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, sourceAssembly: "ESF.Gameplay")]
    public class TowerAttackRangeConfigData : ITowerData
    {
        [SerializeField] private float _range;

        public float Range => _range;
    }
}