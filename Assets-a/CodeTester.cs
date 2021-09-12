using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using Cinemachine;

[SaveDuringPlay]
public class CodeTester : MonoBehaviour
{



   

   
   
    /* object CompileToObject(string classCode)
     {
         CompilerResults result = provider.CompileAssemblyFromSource(parameters, classCode);

         if (result.Errors.Count > 0)
         {
             if (result.Errors.Count > 0)
             {
                 foreach (CompilerError CompErr in result.Errors)
                 {
                     Debug.Log(
                          "Line number " + CompErr.Line +
                          ", Error Number: " + CompErr.ErrorNumber +
                          ", '" + CompErr.ErrorText + ";" +
                          Environment.NewLine + Environment.NewLine);
                 }
             }
             return null;
         }

         var ass = result.CompiledAssembly;

         Type[] types = ass.GetTypes();

         string programType;
         foreach (Type t in types)
         {

             return ass.CreateInstance(typeof(object).FullName);

         }
         return null;
     }
     // Update is called once per frame
     void Update()
     {

     }*/
}