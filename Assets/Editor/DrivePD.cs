using Libraries.system.file_system;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(DriveSO))]
public class DrivePD : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0; //idk why
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        EditorGUILayout.PropertyField(property, GUIContent.none);
        DriveSO mainObject = ((DriveSO)property.GetTargetObjectOfProperty());
        if (mainObject == null)
        {
            GUILayout.Box("DriveSO is null");
            return;
        }

        if (GUILayout.Button("Open Editor"))
        {
            mainObject.GenerateCacheData();
            mainObject.OpenEditor();
        }

        if (GUILayout.Button("Generate parent links"))
        {
            mainObject.GenerateCacheData();
        }

        EditorGUILayout.EndHorizontal();
    }
}