using UnityEditor;
using UnityEngine;

namespace ESF.Core.SerializedInterfaces.Editor
{
    [CustomPropertyDrawer(typeof(SerializedInterface<>))]
    public class SerializedInterfacePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetProperty = property.FindPropertyRelative("_target");
            EditorGUI.PropertyField(position, targetProperty, label, true);
        }
    }
}