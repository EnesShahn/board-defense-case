using System;
using System.Collections.Generic;

namespace ESF.Core.GenericComponentContainer
{
    // Keep clean, no validation for now
    public class ComponentContainer<TBase> where TBase : class
    {
        private readonly Dictionary<Type, TBase> _components = new();

        public bool TryGet<T>(out T component) where T : class, TBase
        {
            bool success = _components.TryGetValue(typeof(T), out TBase tBase);
            if (success)
                component = tBase as T;
            else
                component = null;
            return success;
        }

        public T Get<T>() where T : class, TBase =>
            _components.TryGetValue(typeof(T), out var component) ? component as T : null;
        public bool Contains<T>() where T : TBase =>
            _components.ContainsKey(typeof(T));
        public void Add<T>(T component) where T : class, TBase =>
            _components.Add(typeof(T), component);
        public void Add(TBase component, Type type) =>
            _components.Add(type, component);
        public void Remove<T>(T component) where T : class, TBase =>
            _components.Remove(typeof(T));
    }
}