using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ARC.SerializationToolkit.Runtime.Dictionaries;
using ARC.SerializationToolkit.Dictionaries;
using SerializationToolkit.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace SerializationToolkit.Dictionaries.Internal
{
    [CustomPropertyDrawer(typeof(UDictionary<,,>), true)]
    [CustomPropertyDrawer(typeof(UDictionary<,>), true)]
    public class UDictionaryPropertyDrawer : PropertyDrawer
    {
        private struct ViewData
        {
            public RList List;
            public ListSettingsAttribute Settings;
            public ViewData(SerializedProperty property, ListSettingsAttribute attribute)
            {
                List = RList.Create(property);
                Settings = attribute;
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
                m_PerInstanceData[key] = new ViewData(prop,
                    this.fieldInfo.GetCustomAttributes<ListSettingsAttribute>().FirstOrDefault() ?? new ListSettingsAttribute());

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
            var viewData = GetViewData(property);
            using (new LabelWidth(viewData.Settings.LabelWidth)){
                if(label?.text != null)
                    viewData.List.LabelContent = label.text;
                viewData.List.ItemPrefix = viewData.Settings.ItemPrefix;
                viewData.List.OnGUI(position);
            }
        }
    }
}