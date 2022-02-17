using System;
using System.Collections;
using System.Collections.Generic;
using Libraries.system;
using Libraries.system.file_system;
using NaughtyAttributes;
using UnityEngine;

public class FileHub : MonoBehaviour
{
    public DriveSO drive;
    public List<FileLink> links = new List<FileLink>();

    [Button]
    public void UpdateAllData()
    {
    }

    [Button]
    public void AddDriveLinks()
    {
        foreach (var fileLink in links)
        {
            fileLink.drive = drive;
        }
    }
}

[Serializable]
public class FileLink
{
    public string path;

    public TextAsset asset;

    /* [HideInInspector] */
    public DriveSO drive;


    public void UpdateData()
    {
        if (path == null || asset == null || drive == null)
        {
            return;
        }

        File f = drive.drive.GetFileByPath(path);
        if (f == null)
        {
            return;
        }

        f.data = Runtime.StringToEncodedBytes(asset.text);
        Debug.Log("Updated data");
    }
}