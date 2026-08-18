namespace ESF.Core.Services
{
    // a facade to global service container...
    public static class Service
    {
        internal static readonly ServiceContainer s_serviceContainer = new();

        public static ServiceContainer ServiceContainer => s_serviceContainer;
        public static ReadonlyServiceContainer ReadonlyServiceContainer => new ReadonlyServiceContainer(s_serviceContainer);

        public static OperationResults.RegisterResult Register<TContract>(TContract service) where TContract : class
        {
            return s_serviceContainer.Register<TContract>(service);
        }
        public static OperationResults.RegisterResult Register<TContract, TConcrete>(TConcrete service) where TContract : class where TConcrete : class, TContract
        {
            return s_serviceContainer.Register<TContract, TConcrete>(service);
        }
        public static OperationResults.UnregisterResult Unregister<TContract>() where TContract : class
        {
            return s_serviceContainer.Unregister<TContract>();
        }
        public static OperationResults.ResolveResult TryResolve<TContract>(out TContract service) where TContract : class
        {
            return s_serviceContainer.TryResolve<TContract>(out service);
        }
        public static TContract Resolve<TContract>() where TContract : class
        {
            return s_serviceContainer.Resolve<TContract>();
        }

        public static bool ServiceExists<TContract>() where TContract : class
        {
            return s_serviceContainer.ServiceExists<TContract>();
        }
    }
}