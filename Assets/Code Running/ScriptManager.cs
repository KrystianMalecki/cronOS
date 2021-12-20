using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Collections.Concurrent;
using Libraries.system.file_system;
using System.Linq;
using System.Text.RegularExpressions;
using helper;
using System.Xml;
//todo -9 copy!
public class ScriptManager
{

    [SerializeField]
    private ConcurrentQueue<ITryToRun> actionQueue = new ConcurrentQueue<ITryToRun>();
    [SerializeField]

    public List<CodeTask> scriptsRunning = new List<CodeTask>();


   
   
  
  








 



   
}
