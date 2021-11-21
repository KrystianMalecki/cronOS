using Libraries.system.file_system;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Drive", menuName = "ScriptableObjects/Drive")]

//[Serializable]
public class DriveSO : SerializedScriptableObject
{
    [NonSerialized, OdinSerialize]
 //  [SerializeField]
    public File root;
    //  public Dictionary<string, File> pathLinks = new Dictionary<string, File>();

    [NaughtyAttributes.Button]
    public void OpenEditor()
    {
        GenerateParentLinks();

        SerializedObject so = new SerializedObject(this, this);
        FileEditor.ShowWindow(root, so.FindProperty("root"), so);
    }
    [NaughtyAttributes.Button]
    public void GenerateParentLinks()
    {
        root.GenerateParentLinks(true);
    }


}
