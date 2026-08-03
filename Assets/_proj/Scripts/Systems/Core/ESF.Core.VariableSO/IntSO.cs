using UnityEngine;

namespace ESF.Core.VariableSO
{
    [CreateAssetMenu(menuName = "ESF/Variable SO/Int")]
    public class IntSO : ScriptableObject
    {
        [SerializeField] protected int _value;

        public int Value => _value;

        public static implicit operator int(IntSO intSo) => intSo._value;
    }
}