using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;


[SaveDuringPlay]
public class DebugCodeRunner : MonoBehaviour
{
    public bool runOnStart;
    public TextAsset codeFile = null;

    [ShowIf("noCodeFile")] [Foldout("Code")] [ResizableTextArea]
    public string code;

    bool noCodeFile => (codeFile == null || string.IsNullOrEmpty(codeFile?.text));

    public void Start()
    {
        if (codeFile == null)
        {
#if UNITY_EDITOR
            codeFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Dumpster/empty.txt");
#endif
        }

        if (runOnStart)
        {
            RunCode();
        }
    }

    [Button("Run code", EButtonEnableMode.Playmode)]
    void RunCode()
    {
        PCLogic.defaultInstance.hardware.hardwareInternal.RunCode(new CodeObject(
            noCodeFile ? code : codeFile.text.Replace("false//changeToTrue", "true"),
            HardwareInternal.allLibraryDatas));
    }

    void KillAll()
    {
        PCLogic.defaultInstance.hardware.hardwareInternal.KillAll();
    }
}