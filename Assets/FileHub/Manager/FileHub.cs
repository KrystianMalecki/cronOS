using Libraries.system;
using Libraries.system.file_system;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FileHub : MonoBehaviour
{
    public DriveSO drive;
    public List<FileLink> links = new List<FileLink>();
    [HorizontalLine][SerializeField] private string path;
    private const string folderPath = "/FileHub/";

    [Button]
    private void MakeLink()
    {
        drive.GenerateCacheData();
        File file = drive.drive.GetFileByPath(path);
        string rawName = Application.dataPath + folderPath + file.name;
        string data = "#if false\n" + Runtime.BytesToEncodedString(file.data)
            .Replace("#include", "//#include")
            .Replace("#top", "//#top")
            //.Replace("#include", "//#include")
            + "\n#endif";

        System.IO.File.WriteAllText(rawName + ".txt", data);
        System.IO.File.Move(rawName + ".txt", rawName + ".cs");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        TextAsset textAsset =
            AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/" + folderPath + file.name + ".cs");
        FileLink link = new FileLink(file.GetFullPath(), textAsset, drive);
        links.Add(link);
    }

    [Button]
    public void UpdateAllData()
    {
        foreach (FileLink link in links)
        {
            link.UpdateData();
        }
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
        string loadedText = asset.text
            .Replace("#if true", "")
            .Replace("#if false", "")
            .Replace("#endif", "")
            .Replace("//#", "#")
            .Trim();
        f.data = Runtime.StringToEncodedBytes(loadedText);
        Debug.Log("Updated data");
    }
}