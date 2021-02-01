using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SerializationToolkit
{
    public class RList
    {
        private ReorderableList m_ReorderableList;

        private SerializedProperty Property;
        public string LabelContent { get; set; }

        public string ItemPrefix { get; set; } = "Item";

        public float GetHeight() => m_ReorderableList.GetHeight();

        public bool Draggable
        {
            get => m_ReorderableList.draggable;
            set => m_ReorderableList.draggable = value;
        }


        public static RList Create(SerializedObject serializedObject, string propertyName) =>
            new RList(serializedObject, serializedObject.FindProperty(propertyName));

        public static RList Create(SerializedProperty serializedProperty) =>
            new RList(serializedProperty.serializedObject, serializedProperty);

        private RList(SerializedObject serializedObject, SerializedProperty serializedProperty)
        {
            this.Property = serializedProperty;
            m_ReorderableList = new ReorderableList(serializedObject, serializedProperty)
            {
                drawElementCallback = DrawListItems, 
                drawHeaderCallback = DrawListHeader,
                elementHeightCallback = ElementHeightCallback
            };
        }

        private float ElementHeightCallback(int index)
        {
            if (!Property.isExpanded) return 0;
            return EditorGUI.GetPropertyHeight(Property.GetArrayElementAtIndex(index));
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            var contentPosition = new Rect(rect);
            contentPosition.x += 10f;
            contentPosition.width -= 10f;
            if (Property.isExpanded == false)
            {
                GUI.enabled = index == (m_ReorderableList.count - 1);
                return;
            }

            var element = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(contentPosition, element, new GUIContent(ItemPrefix + (index + 1).ToString()), true);
        }

        private void DrawListHeader(Rect rect)
        {
            Property.isExpanded = EditorGUI.Foldout(rect, Property.isExpanded, new GUIContent(LabelContent ?? Property.displayName));
        }

        public void OnInspectorGUI() => m_ReorderableList?.DoLayoutList();

        public void OnGUI(Rect rect)
        {
            if (m_ReorderableList == null)
                return;
            m_ReorderableList.DoList(rect);
        }
    }
}