using UnityEngine;

namespace ESF.Utilities.Math
{
    //TODO: Can be optimized
    public static class EBezierCurve
    {
        public static Vector3 QuadBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return new Vector3(
                (1 - t) * (1 - t) * p0.x + 2 * (1 - t) * t * p1.x + t * t * p2.x,
                (1 - t) * (1 - t) * p0.y + 2 * (1 - t) * t * p1.y + t * t * p2.y,
                (1 - t) * (1 - t) * p0.z + 2 * (1 - t) * t * p1.z + t * t * p2.z);
        }

        public static Vector3 CubeBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return new Vector3(
                (1 - t) * (1 - t) * (1 - t) * p0.x + 3 * (1 - t) * (1 - t) * t * p1.x + 3 * (1 - t) * t * t * p2.x + t * t * t * p3.x,
                (1 - t) * (1 - t) * (1 - t) * p0.y + 3 * (1 - t) * (1 - t) * t * p1.y + 3 * (1 - t) * t * t * p2.y + t * t * t * p3.y,
                (1 - t) * (1 - t) * (1 - t) * p0.z + 3 * (1 - t) * (1 - t) * t * p1.z + 3 * (1 - t) * t * t * p2.z + t * t * t * p3.z);
        }

        // public static float EaseOutBack(float t)
        // {
        //     float c1 = 1.70158f;
        //     float c3 = c1 + 1;
        //
        //     return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
        // }
    }
}