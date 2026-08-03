using System;
using UnityEngine;
using Game.TowerCells;
using Game.Towers.Root;

namespace Game.Towers.Components
{
    public class TowerCellReference : MonoBehaviour, ITowerComponent
    {
        [SerializeField] private TowerRoot _towerRoot;

        private TowerCellRoot _towerCellAttachedTo;

        public TowerCellRoot TowerCellAttachedTo => _towerCellAttachedTo;
        public TowerRoot TowerRoot => _towerRoot;

        public event Action<TowerCellReference> AttachedToTowerCell;
        public event Action<TowerCellReference> DetachedFromTowerCell;

        public void AttachToTowerRoot(TowerCellRoot towerCellRoot)
        {
            if (_towerCellAttachedTo != null)
            {
                Debug.LogError("a Tower Root is already attached to this tower cell.");
                return;
            }

            _towerCellAttachedTo = towerCellRoot;
            AttachedToTowerCell?.Invoke(this);
        }
        public void DetachFromTowerRoot()
        {
            if (_towerCellAttachedTo == null)
                return;

            _towerCellAttachedTo = null;
            DetachedFromTowerCell?.Invoke(this);
        }
    }
}