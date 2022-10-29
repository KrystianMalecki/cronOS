using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using Libraries.system;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;


[SaveDuringPlay]
public class DebugCodeRunner : MonoBehaviour
{
    public bool runOnStart;
    public TextAsset codeFile = null;

    [ShowIf("noCodeFile")]
    [Foldout("Code")]
    [ResizableTextArea]
    public string code;

    bool noCodeFile => (codeFile == null || string.IsNullOrEmpty(codeFile?.text));


    public void Start()
    {
        Debug.Log(typeof(UnityEngine.MonoBehaviour).GetTypeInfo().Assembly + " == " +
            typeof(UnityEngine.Vector2).GetTypeInfo().Assembly);
        if (codeFile == null)
        {
#if UNITY_EDITOR
            codeFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Dumpster/empty.txt");
#endif
        }

        if (runOnStart)
        {
            DebugCode();
        }
    }

    [Button("Run code", EButtonEnableMode.Playmode)]
    void DebugCode()
    {
        PlayerController.selectedPC.hardwareInternal.Compile(Drive.MakeFile("debugFile",
         Runtime.StringToEncodedBytes(noCodeFile ? code : codeFile.text.Replace("false//changeToTrue", "true"))));
    }

    void KillAll()
    {
        PlayerController.selectedPC.hardwareInternal.KillAll();
    }
}