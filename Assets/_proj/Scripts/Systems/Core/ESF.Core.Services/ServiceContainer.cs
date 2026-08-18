using System;
using System.Collections.Generic;

namespace ESF.Core.Services
{
    // Basically a class containing instances of other classes, or itself if you (for scoping), every instance is associated with a Type,
    // could be its own type or parent type (base class/interface), you can only have one registered instance per Type
    public class ServiceContainer
    {
        internal readonly Dictionary<Type, object> _servicesMap = new();

        public OperationResults.RegisterResult Register<TContract>(TContract service) where TContract : class
        {
            return RegisterInternal<TContract>(service);
        }
        public OperationResults.RegisterResult Register<TContract, TConcrete>(TConcrete service) where TContract : class where TConcrete : class, TContract
        {
            return RegisterInternal<TContract>(service);
        }
        public OperationResults.UnregisterResult Unregister<TContract>() where TContract : class
        {
            return UnregisterInternal<TContract>();
        }
        public OperationResults.ResolveResult TryResolve<TContract>(out TContract service) where TContract : class
        {
            return TryResolveInternal<TContract>(out service);
        }
        public TContract Resolve<TContract>() where TContract : class
        {
            var resolveResult = TryResolveInternal<TContract>(out var service);
            switch (resolveResult)
            {
                case OperationResults.ResolveResult.Success:
                    return service;
                case OperationResults.ResolveResult.ServiceDoesntExist:
                    throw new ServiceNotFoundException($"Service of type {typeof(TContract)} doesn't exist.");
                default:
                    throw new ServiceNotFoundException("If you see this Exception, then run as fast as you can, your device is about to become sentient.");
            }
        }
        
        public bool ServiceExists<TContract>() where TContract : class
        {
            return _servicesMap.ContainsKey(typeof(TContract));
        }

        public void ClearAllServices()
        {
            _servicesMap.Clear();
        }

        internal OperationResults.RegisterResult RegisterInternal<TContract>(object service) where TContract : class
        {
            Type itemType = typeof(TContract);
            if (_servicesMap.ContainsKey(itemType))
            {
                return OperationResults.RegisterResult.ServiceAlreadyRegistered;
            }

            _servicesMap.Add(itemType, service);
            return OperationResults.RegisterResult.Success;
        }

        internal OperationResults.ResolveResult TryResolveInternal<TContract>(out TContract service) where TContract : class
        {
            service = null;

            Type itemType = typeof(TContract);
            if (!_servicesMap.ContainsKey(itemType))
            {
                return OperationResults.ResolveResult.ServiceDoesntExist;
            }

            service = (TContract)_servicesMap[itemType];
            return OperationResults.ResolveResult.Success;
        }

        internal OperationResults.UnregisterResult UnregisterInternal<TContract>() where TContract : class
        {
            Type itemType = typeof(TContract);

            if (_servicesMap.Remove(itemType))
                return OperationResults.UnregisterResult.Success;

            return OperationResults.UnregisterResult.ServiceDoesntExist;
        }
    }
}