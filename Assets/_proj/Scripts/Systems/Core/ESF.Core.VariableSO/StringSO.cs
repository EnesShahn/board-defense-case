using UnityEngine;

namespace ESF.Core.VariableSO
{
    [CreateAssetMenu(menuName = "ESF/Variable SO/String")]
    public class StringSO : ScriptableObject
    {
        [SerializeField] private string _value;

        public string Value => _value;

        public static implicit operator string(StringSO stringSO) => stringSO._value;
    }
}