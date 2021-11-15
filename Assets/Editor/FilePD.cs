using Libraries.system.file_system;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
[CustomPropertyDrawer(typeof(File))]
public class FilePD : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property == null)
        {
            return 0;
        }
        Init(property);
        float childredHeight = 0;
        try
        {
            childredHeight = EditorGUI.GetPropertyHeight(childrenSP);
        }
        catch (Exception e)
        {

        }
        return EditorGUIUtility.singleLineHeight
            + childredHeight
        + EditorGUI.GetPropertyHeight(permissionsSP)
        ;
    }
    SerializedProperty nameSP;
    SerializedProperty permissionsSP;
    SerializedProperty childrenSP;
    public void Init(SerializedProperty property)
    {

        nameSP = property.FindPropertyRelative("name");
        permissionsSP = property.FindPropertyRelative("permissions");
        childrenSP = property.FindPropertyRelative("children");


    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property == null)
        {
            return;
        }

        Init(property);
        var main = new Rect(position.x, position.y, position.width, position.height);
        var nameRect = new Rect(main.x, main.y, main.width / 2, EditorGUIUtility.singleLineHeight);
        var buttonRect = new Rect(main.x + (main.width / 2) + 20, main.y, main.width / 2 - 20, EditorGUIUtility.singleLineHeight);


        var permissionsRect = new Rect(main.x, main.y + EditorGUIUtility.singleLineHeight, main.width, EditorGUI.GetPropertyHeight(permissionsSP));
        var filesRect = new Rect(main.x + 2, permissionsRect.y + permissionsRect.height, main.width - 2, EditorGUI.GetPropertyHeight(childrenSP));

        var button = new Rect(position.x + position.width * 0.25f, position.y + position.height - EditorGUIUtility.singleLineHeight - 6, position.width * 0.5f, EditorGUIUtility.singleLineHeight);

        var line = new Rect(position.x + position.width * 0.05f, button.y + EditorGUIUtility.singleLineHeight + 2, position.width * 0.9f, 2);
        //EditorGUI.PropertyField(main, property, label, true);
        // EditorGUI.indentLevel--;
        EditorGUI.PropertyField(nameRect, nameSP, GUIContent.none);
        //  permissionsSP.intValue = ((int)((FilePermission)EditorGUI.EnumFlagsField(permissionsRect, (FilePermission)permissionsSP.intValue)));
        EditorGUI.PropertyField(permissionsRect, permissionsSP);




        GUI.color = new Color(1.1f, 1.1f, 1.1f);
        try
        {
            EditorGUI.PropertyField(filesRect, childrenSP);
        }
        catch (Exception e)
        {

        }

        GUI.color = Color.white;
        // EditorGUI.indentLevel++;

        if (GUI.Button(buttonRect, "Open in editor"))
        {
            ((DriveSO)property.serializedObject.targetObject).GenerateParentLinks();
            FileEditor.DisplayCurrentFile(property.GetTargetObjectOfProperty() as File, property, property.serializedObject);
        }
        property.serializedObject.ApplyModifiedProperties();

        // EditorGUI.DrawRect(line, Color.gray);


    }
}
