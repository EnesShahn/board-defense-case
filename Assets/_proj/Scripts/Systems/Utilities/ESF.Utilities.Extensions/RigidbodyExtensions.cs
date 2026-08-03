using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ESF.Utilities.Extensions
{
    public static class RigidbodyExtensions
    {
        public static Rigidbody Kinematic(this Rigidbody rb, bool value)
        {
            if (rb == null) return null;
            rb.isKinematic = value;
            return rb;
        }

        public static Rigidbody Gravity(this Rigidbody rb, bool value)
        {
            if (rb == null) return null;
            rb.useGravity = value;
            return rb;
        }

        public static Rigidbody Interpolation(this Rigidbody rb, RigidbodyInterpolation value)
        {
            if (rb == null) return null;
            rb.interpolation = value;
            return rb;
        }
    }
}