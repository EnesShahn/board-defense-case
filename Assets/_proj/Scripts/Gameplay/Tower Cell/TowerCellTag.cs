using UnityEngine;
using ESF.Core.Tags;

namespace Game.TowerCells
{
    [DisallowMultipleComponent]
    public class TowerCellTag : Tag<TowerCellTag>, ITag
    {
        [SerializeField] private TowerCellRoot _towerCellRoot;

        public TowerCellRoot TowerCellRoot => _towerCellRoot;

        private void OnValidate()
        {
            _towerCellRoot ??= GetComponent<TowerCellRoot>();
            _towerCellRoot ??= GetComponentInParent<TowerCellRoot>();
        }
    }
}