using System;
using UnityEngine;

namespace ESF.Core.DataRepository
{
    [Serializable]
    public class Float3Data
    {
        public float x;
        public float y;
        public float z;

        public Float3Data() { }

        public Float3Data(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Float3Data(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public static implicit operator Float3Data(Vector3 vector3)
        {
            return new Float3Data(vector3.x, vector3.y, vector3.z);
        }

        public static implicit operator Vector3(Float3Data float3Data)
        {
            return new Vector3(float3Data.x, float3Data.y, float3Data.z);
        }

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }
    }
}