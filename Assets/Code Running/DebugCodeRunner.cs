using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using System.Collections;
using InternalLogger;


[SaveDuringPlay]
public class DebugCodeRunner : MonoBehaviour
{


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public static DebugCodeRunner instance;



    [Button("Run code")]
    void RunCode()
    {

        ScriptManager.instance.RunCode(new CodeObject(code, ScriptManager.allLibraries));

    }

    [Button("Kill All")]
    void KillAll()
    {
        ScriptManager.instance.KillAll();
    }
    [ResizableTextArea] public string code;

}
