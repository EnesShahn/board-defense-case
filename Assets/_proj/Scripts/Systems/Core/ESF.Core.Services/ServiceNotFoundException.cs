using System;

namespace ESF.Core.Services
{
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string message) : base(message)
        {
        }
    }
}