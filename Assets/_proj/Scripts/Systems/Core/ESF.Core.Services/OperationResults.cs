namespace ESF.Core.Services
{
    public static class OperationResults
    {
        public enum RegisterResult
        {
            Success,
            ServiceAlreadyRegistered
        }
        public enum ResolveResult
        {
            Success,
            ServiceDoesntExist
        }

        public enum UnregisterResult
        {
            Success,
            ServiceDoesntExist
        }
    }
}