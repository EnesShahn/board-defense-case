using UnityEngine;

namespace ESF.Utilities.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool TryGetChild(this GameObject go, string childName, out GameObject result)
        {
            foreach (Transform child in go.transform)
            {
                if (child.name == childName)
                {
                    result = child.gameObject;
                    return true;
                }
            }

            result = null;
            return false;
        }
        public static GameObject SetActiveInline(this GameObject gameObject, bool value)
        {
            if (gameObject == null)
                return null;
            gameObject.SetActive(value);
            return gameObject;
        }
    }
}