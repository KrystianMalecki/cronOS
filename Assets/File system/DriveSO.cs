using Libraries.system.file_system;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "Drive", menuName = "ScriptableObjects/Drive")]

[System.Serializable]
public class DriveSO : ScriptableObject
{
    [SerializeField]
    public File root;
  //  public Dictionary<string, File> pathLinks = new Dictionary<string, File>();

    [Button]
    public void OpenEditor()
    {
        GenerateParentLinks();

        SerializedObject so = new SerializedObject(this, this);
        FileEditor.ShowWindow(root, so.FindProperty("root"), so);
    }
    [Button]
    public void GenerateParentLinks()
    {
        root.GenerateParentLinks(true);
    }


}
