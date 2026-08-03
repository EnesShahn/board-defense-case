using UnityEngine;
using ESF.Core.GenericComponentContainer;
using ESF.Core.PriorityEvents;
using Game.Enemies.Configs;

namespace Game.Enemies.Root
{
    [DisallowMultipleComponent]
    public class EnemyRoot : MonoBehaviour
    {
        private EnemyConfig _enemyConfig;

        private readonly ComponentContainer<IEnemyComponent> _components = new();
        private bool _isCreatedAndActive;

        public ComponentContainer<IEnemyComponent> Components => _components;
        public EnemyConfig EnemyConfig => _enemyConfig;
        public bool IsCreatedAndActive => _isCreatedAndActive;

        // Called internally from Enemy Config Service. NEVER instantiate enemies manually
        public readonly PriorityEvent<EnemyRoot> EnemyPrePoolAllocate = new PriorityEvent<EnemyRoot>();
        public readonly PriorityEvent<EnemyRoot> EnemyPostPoolAllocate = new PriorityEvent<EnemyRoot>();
        public readonly PriorityEvent<EnemyRoot> EnemyPreCreate = new PriorityEvent<EnemyRoot>();
        public readonly PriorityEvent<EnemyRoot> EnemyPostCreate = new PriorityEvent<EnemyRoot>();
        public readonly PriorityEvent<EnemyRoot> EnemyPreDestroy = new PriorityEvent<EnemyRoot>();
        public readonly PriorityEvent<EnemyRoot> EnemyPostDestroy = new PriorityEvent<EnemyRoot>();

        public T GetComp<T>() where T : class, IEnemyComponent => _components.Get<T>();
        public bool TryGetComp<T>(out T component) where T : class, IEnemyComponent => _components.TryGet<T>(out component);

        public void SetEnemyConfig(EnemyConfig enemyConfig)
        {
            _enemyConfig = enemyConfig;
        }
        public void SetIsCreatedAndActive(bool isCreatedAndActive)
        {
            _isCreatedAndActive = isCreatedAndActive;
        }
    }
}