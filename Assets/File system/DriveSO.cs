using Libraries.system.file_system;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CreateAssetMenu(fileName = "Drive", menuName = "ScriptableObjects/Drive")]

[Serializable]
public class DriveSO : ScriptableObject
{
    // [SerializeField]
    // public File root;
    //  public Dictionary<string, File> pathLinks = new Dictionary<string, File>();
    [SerializeField]
    public ThreadSafeList<File> files = new ThreadSafeList<File>();
    [NaughtyAttributes.Button]
    public void OpenEditor()
    {
        GenerateParentLinks();

        SerializedObject so = new SerializedObject(this, this);
        //  FileEditor.ShowWindow(root, so.FindProperty("root"), so);
    }
    [NaughtyAttributes.Button]
    public void GenerateParentLinks()
    {
        //   root.GenerateParentLinks(true);
    }
    [NaughtyAttributes.Button]
    public void GenerateCacheData()
    {
        // root.GenerateParentLinks(true);
        for (int i = 0; i < files.Count; i++)
        {
            File file = files[i];
            file.drive = this;
            file.parent = files[file.parentID];
            files[file.parentID].AddChild(file);
        }
    }

}
