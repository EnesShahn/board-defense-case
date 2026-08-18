using System.Collections.Generic;
using UnityEngine;

namespace ESF.Core.Tags
{
    public static class TagsExtensions
    {
        public static bool TryGetTag<T>(this GameObject instance, out T tag) where T : class, ITag
        {
            return TagsService.TryGetTag<T>(instance, out tag);
        }
        public static bool HasTag<T>(this GameObject instance) where T : ITag
        {
            return TagsService.HasInstance<T>(instance);
        }
        public static IEnumerable<T> GetInstances<T>() where T : ITag
        {
            foreach (var instance in TagsService.GetInstances<T>())
                yield return (T)instance;
        }
        public static bool HasInstances<T>() where T : ITag
        {
            return TagsService.HasInstances<T>();
        }
    }
}