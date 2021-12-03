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

        return 0;//idk why
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        EditorGUILayout.PropertyField(property, GUIContent.none);
        if (GUILayout.Button("Open Editor"))
        {
            ((Drive)property.GetTargetObjectOfProperty()).GenerateCacheData();

            ((Drive)property.GetTargetObjectOfProperty()).OpenEditor();
        }
        if (GUILayout.Button("Generate parent links"))
        {
            ((Drive)property.GetTargetObjectOfProperty()).GenerateCacheData();


        }
        EditorGUILayout.EndHorizontal();

    }
}
