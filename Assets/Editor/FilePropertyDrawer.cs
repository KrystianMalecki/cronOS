using Libraries.system.filesystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
[CustomPropertyDrawer(typeof(File))]

public class FilePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        return EditorGUI.GetPropertyHeight(property) + (property.isExpanded ? (EditorGUIUtility.singleLineHeight + 10) : 0);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var main = new Rect(position.x, position.y, position.width, position.height);
        var button = new Rect(position.x + position.width * 0.25f, position.y + position.height - EditorGUIUtility.singleLineHeight - 6, position.width * 0.5f, EditorGUIUtility.singleLineHeight);

        var line = new Rect(position.x + position.width * 0.05f, button.y + EditorGUIUtility.singleLineHeight + 2, position.width * 0.9f, 2);

        EditorGUI.PropertyField(main, property, label, true);

        if (property.isExpanded)
        {

            GUI.Button(button, "Open byte editor");
            EditorGUI.DrawRect(line, Color.gray);

        }
    }
}
