using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;

using NaughtyAttributes;
using UnityEngine;



[SaveDuringPlay]
public class DebugCodeRunner : MonoBehaviour
{

    public bool runOnStart;
    public TextAsset codeFile;
    [ShowIf("noCodeFile")]
    [Foldout("Code")]
    [ResizableTextArea] public string code;
    bool noCodeFile => codeFile == null;
    public void Start()
    {

        if (runOnStart)
        {
            RunCode();
        }
    }

    [Button("Run code", EButtonEnableMode.Playmode)]
    void RunCode()
    {
        PCLogic.defaultInstance.hardware.RunCode(new CodeObject(noCodeFile ? code : codeFile.text.Replace("false//changeToTrue","true"), Hardware.allLibraryDatas));

    }

    void KillAll()
    {
        PCLogic.defaultInstance.hardware.KillAll();
    }


}
