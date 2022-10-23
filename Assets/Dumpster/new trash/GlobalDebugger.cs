using System.Collections;
using System.Collections.Generic;
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
    internal void WriteToAsset(string text)
    {
#if UNITY_EDITOR
        File.WriteAllText(assetPath, text);
        EditorUtility.SetDirty(asset);
#endif
    }
    internal void WrapInIf(string text)
    {
        WriteToAsset($"#if false\n{text}\n#endif");
    }
}
