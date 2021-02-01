using UnityEngine;

namespace ARC.SerializationToolkit
{
    public class RListAttribute : PropertyAttribute
    {
        public readonly float LabelWidth;
        public readonly string ItemPrefix;
        public RListAttribute(string itemPrefix = "Item", float labelWidth = -1)
        {
            LabelWidth = labelWidth;
            ItemPrefix = itemPrefix;
        }
    }
}
