using System;
using UnityEngine;
using Game.Cells;
using Game.Towers.Root;

namespace Game.TowerCells
{
    public class TowerCellRoot : MonoBehaviour
    {
        [SerializeField] private CellRoot _cellRoot;
        private TowerRoot _attachedTowerRoot;

        public CellRoot CellRoot => _cellRoot;
        public TowerRoot AttachedTowerRoot => _attachedTowerRoot;

        public event Action<TowerCellRoot> TowerRootAttached;
        public event Action<TowerCellRoot> TowerRootDetached;

        public void AttachTowerRoot(TowerRoot towerRoot)
        {
            if (_attachedTowerRoot != null)
            {
                Debug.LogError("a Tower Root is already attached to this tower cell.");
                return;
            }

            _attachedTowerRoot = towerRoot;
            TowerRootAttached?.Invoke(this);
        }
        public void DetachTowerRoot()
        {
            if (_attachedTowerRoot == null)
                return;

            _attachedTowerRoot = null;
            TowerRootDetached?.Invoke(this);
        }
    }
}