using System;
using ESF.Core.Tags;
using UnityEngine;

namespace Game.Towers.Root
{
    [DisallowMultipleComponent]
    public class TowerTag : Tag<TowerTag>, ITag
    {
        [SerializeField] private TowerRoot _towerRoot;

        public TowerRoot TowerRoot => _towerRoot;

        private void OnValidate()
        {
            _towerRoot ??= GetComponent<TowerRoot>();
            _towerRoot ??= GetComponentInParent<TowerRoot>();
        }
    }
}