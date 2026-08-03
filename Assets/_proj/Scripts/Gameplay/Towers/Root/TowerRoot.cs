using UnityEngine;
using ESF.Core.GenericComponentContainer;
using ESF.Core.PriorityEvents;
using Game.Enemies.Root;
using Game.Towers.Configs;

namespace Game.Towers.Root
{
    public class TowerRoot : MonoBehaviour
    {
        private TowerConfig _towerConfig;

        private readonly ComponentContainer<ITowerComponent> _components = new();
        private bool _isCreatedAndActive;

        public ComponentContainer<ITowerComponent> Components => _components;
        public TowerConfig TowerConfig => _towerConfig;
        public bool IsCreatedAndActive => _isCreatedAndActive;

        // Called internally from Enemy Config Service. NEVER instantiate enemies manually
        public readonly PriorityEvent<TowerRoot> TowerPrePoolAllocate = new PriorityEvent<TowerRoot>();
        public readonly PriorityEvent<TowerRoot> TowerPostPoolAllocate = new PriorityEvent<TowerRoot>();
        public readonly PriorityEvent<TowerRoot> TowerPreCreate = new PriorityEvent<TowerRoot>();
        public readonly PriorityEvent<TowerRoot> TowerPostCreate = new PriorityEvent<TowerRoot>();
        public readonly PriorityEvent<TowerRoot> TowerPreDestroy = new PriorityEvent<TowerRoot>();
        public readonly PriorityEvent<TowerRoot> TowerPostDestroy = new PriorityEvent<TowerRoot>();

        public T GetComp<T>() where T : class, ITowerComponent => _components.Get<T>();
        public bool TryGetComp<T>(out T component) where T : class, ITowerComponent => _components.TryGet<T>(out component);

        public void SetTowerConfig(TowerConfig towerConfig)
        {
            _towerConfig = towerConfig;
        }
        public void SetIsCreatedAndActive(bool isCreatedAndActive)
        {
            _isCreatedAndActive = isCreatedAndActive;
        }
    }
}