using System;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Towers.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, "ESF.Gameplay.Towers", "ESF.Gameplay", "TowerFireDirection")]
    public class TowerFireDirectionConfigData : ITowerData
    {
        [SerializeField] private TowerFireDirectionType _fireDirection;

        public TowerFireDirectionType FireDirection => _fireDirection;
    }
}