using System.Collections.Generic;
using ARC.SerializationToolkit;
using SerializationToolkit.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace SerializationToolkit
{

    [CustomPropertyDrawer(typeof(RListAttribute))]
    public class RListAttributePropertyDrawer : PropertyDrawer
    {
        private struct ViewData
        {
            public RList List;
            public ViewData(SerializedProperty property)
            {
                List = RList.Create(property);
            }
        }
        
        private readonly Dictionary<int, ViewData> m_PerInstanceData = new Dictionary<int,ViewData>();

        private ViewData GetViewData(SerializedProperty property)
        {
            var key = property.propertyPath.GetHashCode();
            if (!m_PerInstanceData.ContainsKey(key) || m_PerInstanceData[key].List == null)
            {
                var prop = property.Copy();
                prop.Next(true);
                m_PerInstanceData[key] = new ViewData(prop);
            }

            return m_PerInstanceData[key];
        } 
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var viewData = GetViewData(property);
            return viewData.List.GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RListAttribute settings = (RListAttribute)attribute;
            
            using (new LabelWidth(settings.LabelWidth))
            {
                var viewData = GetViewData(property);
                if(label?.text != null)
                    viewData.List.LabelContent = label.text;
                viewData.List.ItemPrefix = settings.ItemPrefix;
                viewData.List.OnGUI(position);
            }
        }
    }

}