using UnityEngine;

namespace ESF.Utilities.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 FlattenX(this Vector3 v) => new Vector3(0, v.y, v.z);
        public static Vector3 FlattenY(this Vector3 v) => new Vector3(v.x, 0, v.z);
        public static Vector3 FlattenZ(this Vector3 v) => new Vector3(v.x, v.y, 0);

        public static Vector3 WithX(this Vector3 vec, float value) => new(value, vec.y, vec.z);
        public static Vector3 WithY(this Vector3 vec, float value) => new(vec.x, value, vec.z);
        public static Vector3 WithZ(this Vector3 vec, float value) => new(vec.x, vec.y, value);

        public static Vector3 AddX(this Vector3 vec, float value) => new(vec.x + value, vec.y, vec.z);
        public static Vector3 AddY(this Vector3 vec, float value) => new(vec.x, vec.y + value, vec.z);
        public static Vector3 AddZ(this Vector3 vec, float value) => new(vec.x, vec.y, vec.z + value);

        public static Vector3 MultX(this Vector3 vec, float value) => new(vec.x * value, vec.y, vec.z);
        public static Vector3 MultY(this Vector3 vec, float value) => new(vec.x, vec.y * value, vec.z);
        public static Vector3 MultZ(this Vector3 vec, float value) => new(vec.x, vec.y, vec.z * value);

        public static Vector3 SwitchXZ(this Vector3 vec) => new(vec.z, vec.y, vec.x);
        public static Vector3 SwitchXY(this Vector3 vec) => new(vec.y, vec.x, vec.z);
        public static Vector3 SwitchYZ(this Vector3 vec) => new(vec.x, vec.z, vec.y);

        public static float Volume(this Vector3 vec) => vec.x * vec.y * vec.z;
        public static float DistanceTo(this Vector3 vec, Vector3 target) => (target - vec).magnitude;
        public static float DistanceToSqr(this Vector3 vec, Vector3 target) => (target - vec).sqrMagnitude;
    }
}