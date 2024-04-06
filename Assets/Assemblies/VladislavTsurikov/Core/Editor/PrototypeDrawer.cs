#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.Core.Runtime;
using VladislavTsurikov.Utility.Editor;

namespace VladislavTsurikov.Core.Editor
{
    [CustomPropertyDrawer(typeof(Prototype))]
    public class PrototypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Prototype prototype = property.GetTarget<Prototype>();
            
            prototype.Prefab = (GameObject)EditorGUI.ObjectField(position, "Prefab", prototype.Prefab, typeof(GameObject), false);
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif