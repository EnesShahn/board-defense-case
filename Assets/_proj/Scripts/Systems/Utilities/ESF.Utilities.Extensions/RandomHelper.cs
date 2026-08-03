using System.Collections.Generic;

namespace ESF.Utilities.Extensions
{
    public static class RandomHelper
    {
        public static T GetRandom<T>(this T[] col)
        {
            if (col.Length == 0) return default;
            return col[UnityEngine.Random.Range(0, col.Length)];
        }
        public static T GetRandom<T>(this List<T> col)
        {
            if (col.Count == 0) return default;
            return col[UnityEngine.Random.Range(0, col.Count)];
        }
        public static T GetRandom<T>(this IReadOnlyList<T> col)
        {
            if (col.Count == 0) return default;
            return col[UnityEngine.Random.Range(0, col.Count)];
        }
    }
}