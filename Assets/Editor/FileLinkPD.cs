using System;
using Libraries.system.file_system;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private FileLink fl;
    // private string assetPath;
    // private bool changed;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property, label, true);

        // assetPath = AssetDatabase.GetAssetPath(fl.asset);

        /* if (EditorGUI.EndChangeCheck() || String.IsNullOrEmpty(assetPath) || true)
         {
             assetPath = AssetDatabase.GetAssetPath(fl.asset);
         }*/

        /*MyAllPostprocessor.onPost ??= assets =>
        {
            Debug.Log("on post run");
            if (fl != null)
            {
                Debug.Log(assets.ToFormattedString() + " - " + assetPath);
                if (assets.Contains(assetPath))
                {
                    changed = true;
                }
            }
        };
        Debug.Log($"on post set? {MyAllPostprocessor.onPost != null}");
*/
        position.y += position.height - EditorGUIUtility.singleLineHeight;
        position.width /= 2;
        position.height = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded && true)
        {
            if (fl == null)
            {
                fl = (FileLink)property.GetTargetObjectOfProperty();
            }

            if (fl.drive == null)
            {
                position.width *= 2;
                GUI.Label(position, "Drive is null!");
            }
            else
            {
                //  GUI.color = changed ? Color.red : Color.white;

                if (GUI.Button(position, "Update"))
                {
                    fl = (FileLink)property.GetTargetObjectOfProperty();

                    fl.UpdateData();
                    //   changed = false;
                }

                //GUI.color = Color.white;
                position.x += position.width;
                if (GUI.Button(position, "Open in editor"))
                {
                    fl = (FileLink)property.GetTargetObjectOfProperty();
                    fl.drive.GenerateCacheData();
                    File f = fl.drive.drive.GetFileByPath(fl.path);
                    Debug.Log(
                        $"fl{fl != null} fl.drive{fl.drive != null} fl.drive.drive{fl.drive.drive != null} fl.path{fl.path} f{f != null}");
                    if (f != null)
                    {
                        fl.drive.OpenEditor(f);
                    }
                }
            }
        }
    }
}