using SerializationToolkit.ExtensionMethods;
using JetBrains.Annotations;
using UnityEngine;

namespace ARC.SerializationToolkit.Utility
{
    public static class HandleHelper
    {
        public static Rect WorldToGUIRect(Vector3 worldPos, Vector2 size)
        {
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            return new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y);
        }

        public static void DrawLabel(string text, Vector3 worldPos, Vector2? sizeFactor = null, ColorScheme color = null) 
            => DrawLabel(new GUIContent(text), worldPos, sizeFactor, color);

        public static void DrawLabel(GUIContent content, Vector3 worldPos, Vector2? sizeFactor = null, ColorScheme color = null) {
            UnityEditor.Handles.BeginGUI();
            using (new GUIColor(color))
            {
                Rect labelRect = WorldToGUIRect(worldPos, GUI.skin.label.CalcSize(content) * sizeFactor ?? Vector2.one);
                GUI.Label(labelRect, content);
            }

            UnityEditor.Handles.EndGUI();
        }

        public static bool DrawButton(string text, Vector3 worldPos, Vector2 sizeFactor, ColorScheme color = null) 
            => DrawButton(new GUIContent(text), worldPos, sizeFactor, color);

        public static bool DrawButton(GUIContent content, Vector3 worldPos, Vector2 sizeFactor, ColorScheme color = null) {
            UnityEditor.Handles.BeginGUI();
            
            
            var pushed = false;
            using (new GUIColor(color))
            {
                var style = new GUIStyle(GUI.skin.button);
                style.fontSize = 35;
                if (color != null)
                {
                    style.normal.textColor = color.ContentColor;
                }
                Rect buttonRect =
                    WorldToGUIRect(worldPos, style.CalcSize(content) * sizeFactor);
                
                
                pushed = GUI.Button(buttonRect, content, style);
            }
            UnityEditor.Handles.EndGUI();

            return pushed;
        }
    }
}