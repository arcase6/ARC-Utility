using System;
using UnityEditor;
using UnityEngine;
namespace SerializationToolkit.ExtensionMethods
{
    public readonly struct LabelWidth : IDisposable
    {
        private readonly float m_CachedWidth;
        public LabelWidth(float width)
        {
            m_CachedWidth = EditorGUIUtility.labelWidth;
            if(width > 0)
                EditorGUIUtility.labelWidth = width;
        }

        public void Dispose()
        {
            EditorGUIUtility.labelWidth = m_CachedWidth;
        }
    }

    public class ColorScheme
    {
        public Color Color = GUI.color;
        public Color BackgroundColor = GUI.backgroundColor;
        public Color ContentColor = GUI.contentColor;

        public void Apply()
        {
            GUI.color = Color;
            GUI.backgroundColor = BackgroundColor;
            GUI.contentColor = ContentColor;
        }
    }
    
    public readonly struct GUIColor : IDisposable
    {
        private readonly ColorScheme m_CachedColor;
        public GUIColor(ColorScheme colorScheme = null)
        {
            m_CachedColor = new ColorScheme();
            colorScheme?.Apply();
        }

        public void Dispose()
        { 
            m_CachedColor.Apply();
        }
    }
    
    
    public static class PropertyDrawerExtension
    {
        public static Rect PropertyFields(this Rect contentRect,SerializedProperty root, float labelWidth, params string[] props)
        {
            using (new LabelWidth(labelWidth))
            {
                if (props.Length == 0) return contentRect;
                var nextLine = contentRect;
                nextLine.y += EditorGUIUtility.singleLineHeight + 2f;
                contentRect.width /= props.Length;
                foreach (var propName in props)
                {
                    var index = propName.IndexOf('-');
                    var label = index == -1 ? propName: propName.Substring(0, index); 
                    var name = index == -1 ? propName : propName.Substring(index + 1).Trim(); 
                    var prop = root.FindPropertyRelative(name);
                    if (prop != null)
                        EditorGUI.PropertyField(contentRect, prop, new GUIContent(label));
                    else
                        EditorGUI.LabelField(contentRect, "ERR-" + name);
                    contentRect.x += contentRect.width;
                }

                return nextLine;
            }
        }
    }
    
    public static class EditorExtensionMethods
    {
        public static SerializedProperty GetFirstChild(this SerializedProperty property)
        {
            SerializedProperty prop = property.Copy();
            prop.Next(true);
            return prop;
        }


        public static SerializedProperty GetChildByIndex(this SerializedProperty property, int index)
        {
            SerializedProperty prop = property.GetFirstChild();
            for (int i = 0; i < index; i++)
            {
                prop.Next(false);
            }
            return prop;
        }

        public static SerializedProperty GetNextSibling(this SerializedProperty property)
        {
            SerializedProperty prop = property.Copy();
            prop.Next(false);
            return prop;
        }

        public static SerializedProperty Begin(this SerializedProperty property)
        {
            SerializedProperty start = property.Copy();
            if (property.propertyPath.EndsWith(".Array") == false)
            {
                property.Next(true); //property is a list not an array 
            }
            start.Next(true); //move to Array.size
                              //start.Next(true); //move to Array.data[0]
            return start;
        }

        public static SerializedProperty End(this SerializedProperty property)
        {
            return property.GetEndProperty();
        }

        public static void ForEach(this SerializedProperty arrayProp, Action<SerializedProperty> method)
        {
            int arraySize = arrayProp.arraySize;
            if (arraySize == 0)
            {
                return;
            }
            int index = 0;
            for (SerializedProperty it = arrayProp.GetArrayElementAtIndex(0); index < arraySize; it.Next(false))
            {
                method(it);
                ++index;
            }
        }

        public static void ForEach(this SerializedProperty arrayProp, Action<SerializedProperty, int> method)
        {
            int arraySize = arrayProp.arraySize;
            if (arraySize == 0)
            {
                return;
            }
            int index = 0;
            for (SerializedProperty it = arrayProp.GetArrayElementAtIndex(0); index < arraySize; it.Next(false))
            {
                method(it, index);
                ++index;
            }
        }

        public static void HorizontalBreakRounded(this Editor editor)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        public static void HorizontalBreakRounded(this PropertyDrawer drawer)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        public static void HorizontalBreak(this Editor editor, Color color, float thickness = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, thickness);
            rect.height = thickness;
            EditorGUI.DrawRect(rect, color);
        }
    }
}