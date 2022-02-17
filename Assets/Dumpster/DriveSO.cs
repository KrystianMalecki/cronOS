using Libraries.system.file_system;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Concurrent;

[CreateAssetMenu(fileName = "DriveSO", menuName = "ScriptableObjects/DriveSO")]
[Serializable]
public class DriveSO : ScriptableObject
{
    public Drive drive;

    // [NaughtyAttributes.Button]
    public void OpenEditor()
    {
        OpenEditor(drive.GetRoot());
    }

    public void OpenEditor(File file)
    {
        GenerateCacheData();
#if UNITY_EDITOR
        SerializedObject so = new SerializedObject(this, this);

        FileEditor.DisplayCurrentFile(file, null, null, so);
#endif
    }

    //  [Button]
    public void GenerateCacheData()
    {
        drive.GenerateCacheData();
    }
}