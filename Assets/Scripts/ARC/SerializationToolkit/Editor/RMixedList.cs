using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Content;
using UnityEditorInternal;
using UnityEngine;

namespace SerializationToolkit
{
    public class ElementWrapper
    {
        private readonly SerializedProperty TupleListProperty;
        private int OriginalArrayIndex;

        public int RListIndex
        {
            get
            {
                var prop = TupleListProperty.GetArrayElementAtIndex(OriginalArrayIndex);
                prop.Next(true);
                return prop.intValue;
            }
            set
            {
                var prop = TupleListProperty.GetArrayElementAtIndex(OriginalArrayIndex);
                prop.Next(true);
                prop.intValue = value;
            }
        }

        public SerializedProperty Property
        {
            get
            {
                var prop = TupleListProperty.GetArrayElementAtIndex(OriginalArrayIndex);
                prop.Next(true);
                prop.Next(true);
                return prop;
            }
        }

        public ElementWrapper(SerializedProperty tupleListProperty, int originalArrayIndex)
        {
            TupleListProperty = tupleListProperty;
            this.OriginalArrayIndex = originalArrayIndex;
        }

        public void DrawPropertyField(Rect rect, int index, string labelText)
        {
            if (OriginalArrayIndex >= TupleListProperty.arraySize)
            {
                EditorGUI.LabelField(rect, "references deleted data. Please remove.");
                return;
            }

            var prop = TupleListProperty.GetArrayElementAtIndex(OriginalArrayIndex);
            prop.Next(true);
            prop.intValue = index;
            prop.Next(true);
            EditorGUI.PropertyField(rect, prop, new GUIContent(labelText), true);
        }

        public float GetHeight()
        {
            if (OriginalArrayIndex >= TupleListProperty.arraySize)
                return EditorGUIUtility.singleLineHeight;
            var prop = TupleListProperty.GetArrayElementAtIndex(OriginalArrayIndex);
            prop.Next(true);
            prop.Next(true);
            return EditorGUI.GetPropertyHeight(prop);
        }

        public void RemoveOriginalData()
        {
            this.TupleListProperty.DeleteArrayElementAtIndex(OriginalArrayIndex);
        }

        public void NotifyDeletion(ElementWrapper other)
        {
            if (other != null && other.TupleListProperty == this.TupleListProperty &&
                other.OriginalArrayIndex < this.OriginalArrayIndex)
                this.OriginalArrayIndex--;
        }
    }

    public class MixedListProperty
    {
        public System.Type NestedType;
        public SerializedProperty Property;
    }

    public class RMixedList
    {
        private readonly ReorderableList m_ReorderableList;

        private List<ElementWrapper> m_Elements = new List<ElementWrapper>();
        private MixedListProperty[] m_Properties;

        public string LabelContent { get; set; }
        public string ItemPrefix { get; set; }

        public float GetHeight() => m_ReorderableList.GetHeight();

        public bool Draggable
        {
            get => m_ReorderableList.draggable;
            set => m_ReorderableList.draggable = value;
        }


        /// <summary>
        /// array Properties is a list of tuples that will store the index in the runtime array
        /// </summary>
        /// <param name="label">Display name</param>
        /// <param name="arrayProperties">List of Tuple Array properties</param>
        /// <returns></returns>
        public static RMixedList Create(string label, MixedListProperty[] arrayProperties)
        {         
            return new RMixedList(label, arrayProperties);
        }

        private RMixedList(string label, MixedListProperty[] arrayProperties)
        {
            int totalSize = arrayProperties.Sum(arrayProperty => arrayProperty.Property.arraySize);
            m_Elements = Enumerable.Repeat<ElementWrapper>(null, totalSize).ToList();

            var invalidProperties = new List<ElementWrapper>();
            foreach (var arrayProperty in arrayProperties)
            {
                for (var i = 0; i < arrayProperty.Property.arraySize; ++i)
                {
                    var wrapper = new ElementWrapper(arrayProperty.Property, i);
                    if (wrapper.RListIndex < m_Elements.Count && m_Elements[wrapper.RListIndex] == null)
                    {
                        m_Elements[wrapper.RListIndex] = wrapper;
                    }
                    else if(m_Elements[wrapper.RListIndex] != null)
                    {
                        Debug.LogError("Invalid Mapping in mixed list. 2 values assigned to same index.");
                        invalidProperties.Add(wrapper);
                    }
                }
            }

            foreach (var invalidProperty in invalidProperties)
            {
                RemoveFromBackingArray(invalidProperty);
            }

            m_Elements = m_Elements.Where(e => e != null).ToList();

            m_Properties = arrayProperties;

            LabelContent = label;
            m_ReorderableList = new ReorderableList(m_Elements, typeof(ElementWrapper))
            {
                drawElementCallback = DrawListItems,
                drawHeaderCallback = DrawListHeader,
                elementHeightCallback = ElementHeightCallback,
                onAddDropdownCallback = AddDropdownCallback,
                onRemoveCallback = DoRemoveButton,
                
            };
        }

        private void DoRemoveButton(ReorderableList list)
        {
            //remove actual data from serialized property
            RemoveFromBackingArray(m_Elements[list.index]);

            //remove entry from reorderable list memory
            list.list.RemoveAt(list.index);
            if (list.index >= list.list.Count - 1)
                list.index = list.list.Count - 1;
        }

        private void RemoveFromBackingArray(ElementWrapper element)
        {
            if (element != null)
            {
                SerializedObject obj = element.Property.serializedObject;
                element.RemoveOriginalData();
                foreach (var other in m_Elements)
                {
                    other?.NotifyDeletion(element);
                }

                obj?.ApplyModifiedProperties();

            }
        }

        private float ElementHeightCallback(int index)
        {
            var prop = m_Properties.FirstOrDefault()?.Property;
            if (prop != null && prop.isExpanded == false)
                return 0.0f;
            if (m_Elements == null || index >= m_Elements.Count || m_Elements[index] == null)
                return EditorGUIUtility.singleLineHeight;
            return m_Elements[index].GetHeight();
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            var prop = m_Properties.FirstOrDefault()?.Property;
            if (prop != null && prop.isExpanded == false)
                return;
            var contentPosition = new Rect(rect);
            contentPosition.x += 10f;
            contentPosition.width -= 10f;

            string labelText = ItemPrefix + (index + 1).ToString();
            if (m_Elements[index] != null)
            {
                //contentPosition = EditorGUI.PrefixLabel(contentPosition, new GUIContent(labelText));
                m_Elements[index].DrawPropertyField(contentPosition, index, labelText);
            }
            else
                EditorGUI.LabelField(contentPosition, labelText + " is null! Please delete!");
        }

        private void DrawListHeader(Rect rect)
        {
            Rect contentPosition = new Rect(rect.x + 10, rect.y, rect.width - 10, rect.height);
            var prop = m_Properties.FirstOrDefault()?.Property;
            if(prop != null)
                prop.isExpanded = EditorGUI.Foldout(contentPosition, prop.isExpanded, LabelContent ?? prop.displayName);
            else
                EditorGUI.LabelField(contentPosition, LabelContent);
        }

        private void AddDropdownCallback(Rect buttonRect, ReorderableList list)
        {
            var menu = new GenericMenu();
            foreach (var property in m_Properties)
            {
                menu.AddItem(new GUIContent(property.NestedType.ToString()), false, OnSelectAddMenuItem, property);
            }

            menu.DropDown(buttonRect);
        }

        private void OnSelectAddMenuItem(object obj)
        {
            var userdata = (MixedListProperty) obj;
            var property = userdata.Property;

            var index = property.arraySize;
            property.arraySize += 1;
            property.serializedObject.ApplyModifiedProperties();
            var element = new ElementWrapper(property, index)
            {
                RListIndex = m_Elements.Count
            };
            //m_Elements.Add(element);
            m_ReorderableList.list.Add(element);
        }

        public void OnInspectorGUI()
        {
            if (m_ReorderableList == null)
                return;
            m_ReorderableList.DoLayoutList();
        }

        public void OnGUI(Rect rect)
        {
            if (m_ReorderableList == null)
                return;
            m_ReorderableList.DoList(rect);
        }
    }
}