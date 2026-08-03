using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ESF.Core.SerializedInterfaces
{
    [Serializable]
    public class SerializedInterface<T> : ISerializationCallbackReceiver where T : class
    {
        [SerializeField] private Object _target;

        private T _targetCasted; // lazy runtime cache

        public T Value
        {
            get
            {
                if (_targetCasted == null && _target != null)
                    _targetCasted = _target as T;

                return _targetCasted;
            }
            set
            {
                if (value is T)
                {
                    _target = value as Object;
                    _targetCasted = value;
                }
            }
        }

        private void OnValidate()
        {
            if (_target != null && _target is not T)
                _target = null;
        }

        public void OnBeforeSerialize() => this.OnValidate();
        public void OnAfterDeserialize() { }
    }
}