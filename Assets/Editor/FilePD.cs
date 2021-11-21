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
            Debug.Log("UH!");
            return 0;
        }
        Init(property);
        float childredHeight = 0;
        //  Debug.Log($"|{nameSP?.GetTargetObjectOfProperty()}-{permissionsSP?.GetTargetObjectOfProperty()}-{childrenSP?.GetTargetObjectOfProperty()}-{childrenItemsSP}|");
        try
        {
            childredHeight = EditorGUI.GetPropertyHeight(childrenSP);
        }
        catch (Exception e)
        {
            //Debug.LogException(e);
        }
        return EditorGUIUtility.singleLineHeight * 2
            + childredHeight
        + EditorGUI.GetPropertyHeight(permissionsSP)
         + (childrenItemsSP.isExpanded ? EditorGUIUtility.singleLineHeight : 0)
        ;
    }
    SerializedProperty nameSP;
    SerializedProperty permissionsSP;
    SerializedProperty childrenSP;
    SerializedProperty childrenItemsSP;

    public void Init(SerializedProperty property)
    {

        nameSP = property.FindPropertyRelative("name");
        permissionsSP = property.FindPropertyRelative("permissions");
        childrenSP = property.FindPropertyRelative("children");
          childrenItemsSP = childrenSP.FindPropertyRelative("items");


    }
    public void say(Rect r)
    {
        Debug.Log($"{nameSP.GetTargetObjectOfProperty()} {r} {r.x} {r.y} {r.width} {r.height}");

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
        var addChildButtonRect = new Rect(main.x, filesRect.y + filesRect.height, main.width, EditorGUIUtility.singleLineHeight);
        Debug.Log("frame");
        say(main);
        say(nameRect);
        say(buttonRect);
        say(permissionsRect);
        say(filesRect);
        say(addChildButtonRect);

        //EditorGUI.PropertyField(main, property, label, true);
        // EditorGUI.indentLevel--;
        EditorGUI.PropertyField(nameRect, nameSP, GUIContent.none);
        //  permissionsSP.intValue = ((int)((FilePermission)EditorGUI.EnumFlagsField(permissionsRect, (FilePermission)permissionsSP.intValue)));
        EditorGUI.PropertyField(permissionsRect, permissionsSP);




        GUI.color = new Color(1.1f, 1.1f, 1.1f);
        try
        {
            EditorGUI.PropertyField(filesRect, childrenSP);
            if (/*childrenItemsSP.isExpanded ||*/ true)
            {
                if (GUI.Button(addChildButtonRect, "Add child"))
                {
                    File file = property.GetTargetObjectOfProperty() as File;
                    file.AddChild(new File());
                    property.serializedObject.Update();
                }
            }
        }
        catch (Exception e)
        {
          //  Debug.LogException(e);
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
