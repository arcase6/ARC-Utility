using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SerializationToolkit;
using SerializationToolkit.ExtensionMethods;
using ICSharpCode.NRefactory.Ast;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MixedList<,,>),true)]
public class MixedListPropertyDrawer : PropertyDrawer
{
    
    //Property Drawer Instances are shared.
    //There is only one instance being used in a single editor for
    //values of the same type
    private struct ViewData
    {
        public RMixedList RMixedList;
        public float LabelWidth;
    }
    
    private Dictionary<int, ViewData> m_PerPropertyViewData = new Dictionary<int,ViewData>();

    private ViewData GetViewData(SerializedProperty property)
    {
        var key = property.propertyPath.GetHashCode();

        if (!m_PerPropertyViewData.ContainsKey(key) || m_PerPropertyViewData[key].RMixedList == null)
            m_PerPropertyViewData[key] = CreateViewData(property);
        return m_PerPropertyViewData[key];
    }

    private ViewData CreateViewData(SerializedProperty property)
    {
        Debug.Log("Initialized : " + this.ToString());
        
        var propertyNames = new string[]{ "m_Group1", "m_Group2"};
        var properties = new List<MixedListProperty>();
        int i = 0;
        foreach (var name in propertyNames)
        {
            var typeGetter = this.fieldInfo.FieldType.GetMethod("GetMemberType",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var groupType = typeGetter?.Invoke(null, new System.Object[]{(object)i}) as System.Type;
            var prop = new MixedListProperty()
            {
                Property =  property.FindPropertyRelative(name),
                NestedType= groupType
            };
            properties.Add(prop);
            i++;
        }

        var settings = this.fieldInfo.GetCustomAttributes<ListSettingsAttribute>().FirstOrDefault() ?? new ListSettingsAttribute();
            
        var viewData = new ViewData
        {
            RMixedList = RMixedList.Create(property.name, properties.ToArray()),
            LabelWidth =  settings.LabelWidth
        };
        viewData.RMixedList.ItemPrefix = settings.ItemPrefix;
        return viewData;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var viewData = GetViewData(property);
        return viewData.RMixedList.GetHeight();
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var viewData = GetViewData(property);
        using (new LabelWidth(viewData.LabelWidth))
        {
            if (label?.text != null)
                viewData.RMixedList.LabelContent = label.text;
            viewData.RMixedList.OnGUI(position);
        }
    }
}
