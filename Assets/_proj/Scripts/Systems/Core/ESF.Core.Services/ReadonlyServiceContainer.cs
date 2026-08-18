using System;
using UnityEngine;

namespace ESF.Core.Services
{
    // Readonly wrapper around ServiceContainer
    public struct ReadonlyServiceContainer
    {
        private ServiceContainer _serviceContainer;

        public ReadonlyServiceContainer(ServiceContainer serviceContainer)
        {
            if (serviceContainer == null)
                throw new ArgumentNullException("serviceContainer is null.");
            _serviceContainer = serviceContainer;
        }

        public OperationResults.ResolveResult TryResolve<TContract>(out TContract service) where TContract : class
        {
            if (_serviceContainer == null)
                Debug.LogError($"[{nameof(TryResolve)}] Service container not initialized");
            return _serviceContainer.TryResolveInternal<TContract>(out service);
        }
        public TContract Resolve<TContract>() where TContract : class
        {
            if (_serviceContainer == null)
                Debug.LogError($"[{nameof(Resolve)}] Service container not initialized");
            return _serviceContainer.Resolve<TContract>();
        }
        public bool ServiceExists<TContract>() where TContract : class
        {
            if (_serviceContainer == null)
                Debug.LogError($"[{nameof(ServiceExists)}] Service container not initialized");
            return _serviceContainer.ServiceExists<TContract>();
        }
    }
}