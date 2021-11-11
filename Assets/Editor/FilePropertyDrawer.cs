using Libraries.system.file_system;
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
        Init(property);

        return EditorGUIUtility.singleLineHeight
            + EditorGUI.GetPropertyHeight(filesSP)
            + EditorGUI.GetPropertyHeight(permissionsSP)
            ;
    }
    SerializedProperty nameSP;
    SerializedProperty permissionsSP;
    SerializedProperty filesSP;
    public void Init(SerializedProperty property)
    {
        nameSP = property.FindPropertyRelative("name");
        permissionsSP = property.FindPropertyRelative("permissions");
        filesSP = property.FindPropertyRelative("files");

    }
    public void TryInit(SerializedProperty property)
    {
        if (filesSP == null)
        {
            Init(property);
        }
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
        var filesRect = new Rect(main.x + 2, permissionsRect.y + EditorGUI.GetPropertyHeight(permissionsSP), main.width - 2, EditorGUI.GetPropertyHeight(filesSP));

        var button = new Rect(position.x + position.width * 0.25f, position.y + position.height - EditorGUIUtility.singleLineHeight - 6, position.width * 0.5f, EditorGUIUtility.singleLineHeight);

        var line = new Rect(position.x + position.width * 0.05f, button.y + EditorGUIUtility.singleLineHeight + 2, position.width * 0.9f, 2);
        //EditorGUI.PropertyField(main, property, label, true);
       // EditorGUI.indentLevel--;
        EditorGUI.PropertyField(nameRect, nameSP, GUIContent.none);
        //  permissionsSP.intValue = ((int)((FilePermission)EditorGUI.EnumFlagsField(permissionsRect, (FilePermission)permissionsSP.intValue)));
        EditorGUI.PropertyField(permissionsRect, permissionsSP);

        GUI.color = new Color(1.1f, 1.1f, 1.1f);
        EditorGUI.PropertyField(filesRect, filesSP);
        GUI.color = Color.white;
       // EditorGUI.indentLevel++;

        if (GUI.Button(buttonRect, "Open in editor"))
        {
            FileEditor.ShowWindow(property.GetTargetObjectOfProperty() as File/*, property,property.serializedObject*/);
        }
        property.serializedObject.ApplyModifiedProperties();

        // EditorGUI.DrawRect(line, Color.gray);


    }
}
