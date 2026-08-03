using CareBoo.Serially;
using ESF.Core.SerializedInterfaces;
using UnityEngine;

namespace Game.Towers.Root
{
    [DefaultExecutionOrder(-1000)]
    public class TowerComponentInstaller : MonoBehaviour
    {
        [SerializeField] private TowerRoot _towerRoot;
        [SerializeField] private SerializedInterface<ITowerComponent> _towerComponent = new();
        [SerializeField] private SerializableType _serializableType;

        private void Awake()
        {
            _towerRoot.Components.Add(_towerComponent.Value, _serializableType.Type);
        }

        private void OnValidate()
        {
            _towerRoot ??= GetComponentInParent<TowerRoot>();
            _towerComponent.Value ??= GetComponent<ITowerComponent>();

            if (_towerComponent.Value != null)
                _serializableType = new SerializableType(_towerComponent.Value.GetType());
        }
    }
}