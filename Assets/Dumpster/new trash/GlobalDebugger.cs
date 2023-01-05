using NaughtyAttributes;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GlobalDebugger : MonoBehaviour
{
    [SerializeField] internal TextAsset asset;
    public static GlobalDebugger instance;
    public static string assetPath;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        assetPath = AssetDatabase.GetAssetPath(asset);
    }
    internal void WriteToDebugFile(string text)
    {
#if UNITY_EDITOR
        File.WriteAllText(assetPath, text);
        EditorUtility.SetDirty(asset);
#endif
    }
    internal void WriteToDebugFileWrapedInIf(string text)
    {
        WriteToDebugFile($"#if false\n{text}\n#endif");
    }
    [Button]
    public static void DisplayLoadedAssembliesNumber()
    {
        Debug.Log($"Loaded assemblies:{AppDomain.CurrentDomain.GetAssemblies().Length}");

    }
    [Button]
    public static void DisplayLoadedAssemblies()
    {
        Debug.Log($"Loaded assemblies:{AppDomain.CurrentDomain.GetAssemblies().ToConvertedString(",\n", x => x.FullName)}");

    }
}
