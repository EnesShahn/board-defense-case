using CareBoo.Serially;
using ESF.Core.SerializedInterfaces;
using UnityEngine;

namespace Game.Enemies.Root
{
    [DefaultExecutionOrder(-1000)]
    public class EnemyComponentInstaller : MonoBehaviour
    {
        [SerializeField] private EnemyRoot _enemyRoot;
        [SerializeField] private SerializedInterface<IEnemyComponent> _enemyComponent = new();
        [SerializeField] private SerializableType _serializableType;

        private void Awake()
        {
            _enemyRoot.Components.Add(_enemyComponent.Value, _serializableType.Type);
        }

        private void OnValidate()
        {
            _enemyRoot ??= GetComponentInParent<EnemyRoot>();
            _enemyComponent.Value ??= GetComponent<IEnemyComponent>();

            if (_enemyComponent.Value != null)
                _serializableType = new SerializableType(_enemyComponent.Value.GetType());
        }
    }
}