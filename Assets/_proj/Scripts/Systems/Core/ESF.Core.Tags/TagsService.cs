using System;
using System.Collections.Generic;
using UnityEngine;

namespace ESF.Core.Tags
{
    internal static class TagsService
    {
        internal class TagData
        {
            //GameObject Hash -> TagComponent
            internal readonly Dictionary<int, ITag> Instances = new(16);
        }

        // Tag type -> TagData
        private static readonly Dictionary<Type, TagData> Tags = new(256);

        public static void AddInstance<T>(Tag<T> tag) where T : ITag
        {
            int hash = tag.gameObject.GetHashCode();
            Type type = typeof(T);
            if (!Tags.ContainsKey(type))
                Tags.Add(type, new());

            Tags[type].Instances.TryAdd(hash, (ITag)tag);
        }
        public static void RemoveInstance<T>(Tag<T> tag) where T : ITag
        {
            int hash = tag.gameObject.GetHashCode();
            Type type = typeof(T);
            if (!Tags.ContainsKey(type))
                return;

            Tags[type].Instances.Remove(hash);
        }

        public static bool HasInstance<T>(GameObject instance) where T : ITag
        {
            int hash = instance.GetHashCode();
            Type type = typeof(T);

            if (!Tags.TryGetValue(type, out var tag))
                return false;

            return tag.Instances.ContainsKey(hash);
        }
        public static bool TryGetTag<T>(GameObject instance, out T tag) where T : class, ITag
        {
            int hash = instance.GetHashCode();
            Type type = typeof(T);
            tag = null;

            if (!Tags.ContainsKey(type))
                return false;

            if (!Tags[type].Instances.TryGetValue(hash, out ITag tagGameObject))
                return false;

            tag = (T)tagGameObject;
            return true;
        }

        public static IEnumerable<T> GetInstances<T>() where T : ITag
        {
            foreach (var instance in Tags[typeof(T)].Instances)
                yield return (T)instance.Value;
        }
        public static bool HasInstances<T>() where T : ITag
        {
            return Tags.ContainsKey(typeof(T));
        }
    }
}