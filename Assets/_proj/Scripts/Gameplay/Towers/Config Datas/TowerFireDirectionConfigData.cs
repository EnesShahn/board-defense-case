using System;
using Game.Towers.Configs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game.Towers.ConfigDatas
{
    [Serializable]
    [MovedFrom(true, sourceAssembly: "ESF.Gameplay")]
    public class TowerFireDirectionConfigData : ITowerData
    {
        [SerializeField] private TowerFireDirectionType _fireDirection;

        public TowerFireDirectionType FireDirection => _fireDirection;
    }
}