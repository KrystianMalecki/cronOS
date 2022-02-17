using Libraries.system.file_system;
using System.Collections;
using System.Collections.Generic;
using helper;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(FileLink))]
public class FileLinkPD : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) + (property.isExpanded ? EditorGUIUtility.singleLineHeight : 0);
        ;
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);

        position.y += position.height - EditorGUIUtility.singleLineHeight;
        position.width /= 2;
        position.height = EditorGUIUtility.singleLineHeight;
        if (property.isExpanded && true)
        {
            if (GUI.Button(position, "Update"))
            {
                (property.GetTargetObjectOfProperty() as FileLink)?.UpdateData();
            }

            position.x += position.width;
            if (GUI.Button(position, "Open in editor"))
            {
                FileLink fl = (FileLink)property.GetTargetObjectOfProperty();

                File f = fl.drive.drive.GetFileByPath(fl.path);
                Debug.Log($"fl{fl} f{f}");
                if (f != null)
                {
                    fl.drive.OpenEditor(f);
                }
            }
        }
    }
}