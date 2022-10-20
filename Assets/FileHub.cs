using System;
using System.Collections;
using System.Collections.Generic;
using Libraries.system;
using Libraries.system.file_system;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class FileHub : MonoBehaviour
{
    public DriveSO drive;
    public List<FileLink> links = new List<FileLink>();
    [HorizontalLine] [SerializeField] private string path;
    private const string folderPath = "/FileHub/";

    [Button]
    private void MakeLink()
    {
        drive.GenerateCacheData();
        File file = drive.drive.GetFileByPath(path);
        System.IO.File.WriteAllText(Application.dataPath + folderPath + file.name + ".txt",
            Runtime.BytesToEncodedString(file.data));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        TextAsset textAsset =
            AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/" + folderPath + file.name + ".txt");
        FileLink link = new FileLink(file.GetFullPath(), textAsset, drive);
        links.Add(link);
    }

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

/*public class MyAllPostprocessor : AssetPostprocessor
{
    public static Action<string[]> onPost;

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        //  Debug.Log($"IS post set? {MyAllPostprocessor.onPost != null}");

        onPost?.Invoke(importedAssets);
    }
}*/

[Serializable]
public class FileLink
{
    public string path;

    public TextAsset asset;

    [HideInInspector] public DriveSO drive;


    public FileLink(string path, TextAsset asset, DriveSO drive)
    {
        this.path = path;
        this.asset = asset;
        this.drive = drive;
    }

    public void UpdateData()
    {
        if (path == null || asset == null || drive == null)
        {
            return;
        }

        drive.GenerateCacheData();
        File f = drive.drive.GetFileByPath(path);
        if (f == null)
        {
            return;
        }

        f.data = Runtime.StringToEncodedBytes(asset.text);
        Debug.Log("Updated data");
    }
}