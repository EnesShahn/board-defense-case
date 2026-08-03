using System;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Towers.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, "ESF.Gameplay.Towers", "ESF.Gameplay", "TowerFireRate")]
    public class TowerFireRateConfigData : ITowerData
    {
        [SerializeField] private float _fireRate;

        public float FireRate => _fireRate;
    }
}