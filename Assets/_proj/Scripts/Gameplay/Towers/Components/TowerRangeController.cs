using ESF.Utilities.Extensions;
using Game.Towers.ConfigDatas;
using Game.Towers.Root;
using UnityEngine;

namespace Game.Towers.Components
{
    public class TowerRangeController : MonoBehaviour, ITowerComponent
    {
        [SerializeField] private TowerRoot _towerRoot;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private SphereCollider _sphereCollider;

        private TowerAttackRangeConfigData _towerAttackRangeConfigData;
        private TowerFireDirectionConfigData _towerFireDirectionConfigData;

        public TowerRoot TowerRoot => _towerRoot;

        private void Awake()
        {
            _towerAttackRangeConfigData = _towerRoot.TowerConfig.GetTowerData<TowerAttackRangeConfigData>();
            _towerFireDirectionConfigData = _towerRoot.TowerConfig.GetTowerData<TowerFireDirectionConfigData>();

            var attackRange = _towerAttackRangeConfigData.Range;

            if (_towerFireDirectionConfigData.FireDirection == TowerFireDirectionType.ForwardOnly)
            {
                _sphereCollider.enabled = false;
                _boxCollider.enabled = true;
                _boxCollider.size = _boxCollider.size.WithZ(attackRange);
                _boxCollider.center = _boxCollider.center.WithZ(attackRange / 2);
            }
            else if (_towerFireDirectionConfigData.FireDirection == TowerFireDirectionType.OmniDirectional)
            {
                _sphereCollider.enabled = true;
                _boxCollider.enabled = false;
                _sphereCollider.radius = attackRange;
            }
        }
    }
}