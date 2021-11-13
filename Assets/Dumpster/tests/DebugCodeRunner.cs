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
    [Foldout("Code")]
    [ResizableTextArea] public string code;
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

        ScriptManager.instance.RunCode(new CodeObject(code, ScriptManager.allLibraryDatas));

    }

    void KillAll()
    {
        ScriptManager.instance.KillAll();
    }


}
