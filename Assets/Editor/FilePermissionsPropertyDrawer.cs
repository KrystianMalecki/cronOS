
using NaughtyAttributes;
using NaughtyAttributes.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;/*
[CustomPropertyDrawer(typeof(FilePermissions))]

public class FilePermissionsPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var main = new Rect(position.x, position.y, position.width, position.height);
        float width = main.width / 5;
        float spacing = main.width / 20;

        var first = new Rect(main.x, main.y, width, main.height);
        var firstLabel = new Rect(main.x + EditorGUIUtility.singleLineHeight, main.y, width, main.height);

        var second = new Rect(first.xMax + spacing, main.y, width, main.height);
        var secondLabel = new Rect(second.x + EditorGUIUtility.singleLineHeight, main.y, width, main.height);
        var third = new Rect(second.xMax + spacing, main.y, width, main.height);
        var thirdLabel = new Rect(third.x + EditorGUIUtility.singleLineHeight, main.y, width, main.height);

        var forth = new Rect(third.xMax + spacing, main.y, width, main.height);
        var forthLabel = new Rect(forth.x + EditorGUIUtility.singleLineHeight, main.y, width, main.height);

        if (width > 50)
        {
            EditorGUI.LabelField(firstLabel, "isFolder");
        }
        EditorGUI.PropertyField(first, property.FindPropertyRelative("isFolder"), GUIContent.none);
        if (width > 40)
        {
            EditorGUI.LabelField(secondLabel, "read");
        }
        EditorGUI.PropertyField(second, property.FindPropertyRelative("read"), GUIContent.none);
        if (width > 40)
        {
            EditorGUI.LabelField(thirdLabel, "write");
        }
        EditorGUI.PropertyField(third, property.FindPropertyRelative("write"), GUIContent.none);
        if (width > 50)
        {
            EditorGUI.LabelField(forthLabel, "execute");
        }
        EditorGUI.PropertyField(forth, property.FindPropertyRelative("execute"), GUIContent.none);

    }

}*/
//[CustomPropertyDrawer(typeof(FilePermission))]

/*public class FilePermissionsPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var main = new Rect(position.x, position.y, position.width, position.height);
        float width = main.width / 5;
        float spacing = main.width / 20;

        var first = new Rect(main.x, main.y, width, main.height);
        var firstLabel = new Rect(main.x + EditorGUIUtility.singleLineHeight, main.y, width, main.height);

        var second = new Rect(first.xMax + spacing, main.y, width, main.height);
        var secondLabel = new Rect(second.x + EditorGUIUtility.singleLineHeight, main.y, width, main.height);
        var third = new Rect(second.xMax + spacing, main.y, width, main.height);
        var thirdLabel = new Rect(third.x + EditorGUIUtility.singleLineHeight, main.y, width, main.height);

        var forth = new Rect(third.xMax + spacing, main.y, width, main.height);
        var forthLabel = new Rect(forth.x + EditorGUIUtility.singleLineHeight, main.y, width, main.height);
        FilePermission fp = ((FilePermission)property.intValue);
        if (width > 50)
        {
            EditorGUI.LabelField(firstLabel, "isFolder");
        }
        if (EditorGUI.Toggle(first, fp.HasFlag(FilePermission.isFolder)))
        {
            fp |= FilePermission.isFolder;
        }
        else
        {
            fp &= ~FilePermission.isFolder;
        }


        if (width > 40)
        {
            EditorGUI.LabelField(secondLabel, "read");
        }
        if (EditorGUI.Toggle(second, fp.HasFlag(FilePermission.read)))
        {
            fp |= FilePermission.read;
        }
        else
        {
            fp &= ~FilePermission.read;
        }

        if (width > 40)
        {
            EditorGUI.LabelField(thirdLabel, "write");
        }
        if (EditorGUI.Toggle(third, fp.HasFlag(FilePermission.write)))
        {
            fp |= FilePermission.write;
        }
        else
        {
            fp &= ~FilePermission.write;
        }
        if (width > 50)
        {
            EditorGUI.LabelField(forthLabel, "execute");
        }
        if (EditorGUI.Toggle(forth, fp.HasFlag(FilePermission.execute)))
        {
            fp |= FilePermission.execute;
        }
        else
        {
            fp &= ~FilePermission.execute;
        }

        property.intValue = (byte)fp;
        property.serializedObject.ApplyModifiedProperties();
    
    }
}
*/

