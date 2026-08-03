using UnityEngine;

namespace ESF.Core.VariableSO
{
    [CreateAssetMenu(menuName = "ESF/Variable SO/Float")]
    public class FloatSO : ScriptableObject
    {
        [SerializeField] protected float _value;

        public float Value => _value;

        public static implicit operator float(FloatSO floatSo) => floatSo._value;
    }
}