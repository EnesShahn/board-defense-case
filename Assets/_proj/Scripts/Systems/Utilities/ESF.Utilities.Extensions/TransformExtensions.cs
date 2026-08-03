using UnityEngine;

namespace ESF.Utilities.Extensions
{
    public static class TransformExtensions
    {
        // Position
        public static Transform SetPositionInline(this Transform transform, Vector3 position)
        {
            transform.position = position;
            return transform;
        }
        public static Transform SetXPositionInline(this Transform tr, float x, bool worldPos = true)
        {
            if (worldPos)
            {
                var pos = tr.transform.position;
                pos.x = x;
                tr.position = pos;
            }
            else
            {
                var pos = tr.transform.localPosition;
                pos.x = x;
                tr.localPosition = pos;
            }

            return tr;
        }
        public static Transform SetYPositionInline(this Transform tr, float y, bool worldPos = true)
        {
            if (worldPos)
            {
                var pos = tr.transform.position;
                pos.y = y;
                tr.position = pos;
            }
            else
            {
                var pos = tr.transform.localPosition;
                pos.y = y;
                tr.localPosition = pos;
            }

            return tr;
        }
        public static Transform SetZPositionInline(this Transform tr, float z, bool worldPos = true)
        {
            if (worldPos)
            {
                var pos = tr.transform.position;
                pos.z = z;
                tr.position = pos;
            }
            else
            {
                var pos = tr.transform.localPosition;
                pos.z = z;
                tr.localPosition = pos;
            }

            return tr;
        }

        // Rotation
        public static Transform SetRotationInline(this Transform transform, Quaternion rotation)
        {
            transform.rotation = rotation;
            return transform;
        }

        // Parent
        public static Transform SetParentInline(this Transform transform, Transform parent, bool worldPositionStays = true, bool resetLocalPosition = true)
        {
            transform.SetParent(parent, worldPositionStays);
            if (resetLocalPosition) transform.localPosition = Vector3.zero;
            return transform;
        }

        public static void DestroyChildren(this Transform transform, bool immediate = false)
        {
            foreach (Transform child in transform)
            {
                if (immediate)
                    GameObject.DestroyImmediate(child.gameObject);
                else
                    GameObject.Destroy(child.gameObject);
            }
        }
        public static void DestroyChildren(this Transform transform, int startIdx)
        {
            DestroyChildren(transform, startIdx, transform.childCount);
        }
        public static void DestroyChildren(this Transform transform, int startIdx, int endIdx)
        {
            if (transform.childCount == 0 || startIdx > transform.childCount - 1) return;

            var children = transform.GetChildren();
            endIdx = Mathf.Min(endIdx, transform.childCount);
            for (int i = startIdx; i < endIdx; i++)
                GameObject.Destroy(children[i].gameObject);
        }

        public static bool TryGetChild(this Transform transform, string childName, out Transform result)
        {
            foreach (Transform child in transform)
            {
                if (child.name == childName)
                {
                    result = child;
                    return true;
                }
            }

            result = null;
            return false;
        }
        public static Transform[] GetChildren(this Transform transform)
        {
            Transform[] children = new Transform[transform.childCount];
            int i = 0;
            foreach (Transform child in transform)
                children[i++] = child;
            return children;
        }

        public static void CenterToChild(this Transform parent, Transform child)
        {
            Vector3 posDiff = child.position - parent.position;
            parent.position += posDiff;
            foreach (Transform childTr in parent)
                childTr.position -= posDiff;
        }
    }
}