using ARC.SerializationToolkit;
using ARC.SerializationToolkit.Dictionaries.Internal;
using UnityEditor;
using UnityEngine;
using SerializationToolkit.ExtensionMethods;

namespace SerializationToolkit.Dictionaries.Internal
{
    [CustomPropertyDrawer(typeof(UKeyValuePair<,>))]
    public class UKeyValuePairPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("Value"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect contentRect = EditorGUI.PrefixLabel(position, label);
            contentRect = contentRect.PropertyFields(property, 30f, "Key", "Value");
        }
    }

}