using Libraries.system.file_system;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
[CustomPropertyDrawer(typeof(Drive))]

public class DrivePD : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        return EditorGUI.GetPropertyHeight(property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect main = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 4, position.width, position.height - EditorGUIUtility.singleLineHeight);
        Rect button = new Rect(position.x + position.width / 4, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
        if (GUI.Button(button, "Validate All"))
        {
            File f = property.FindPropertyRelative("driveFile").GetTargetObjectOfProperty() as File;

            if (f != null)
            {
                f.Validate(true);
            }
        }
        EditorGUI.PropertyField(main, property.FindPropertyRelative("driveFile"), label, true);
        property.serializedObject.ApplyModifiedProperties();
    }
}
