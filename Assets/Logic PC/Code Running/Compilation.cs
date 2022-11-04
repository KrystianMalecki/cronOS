using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.CodeAnalysis;
using System.Threading;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

[System.Serializable]
public class Compilation
{
    [SerializeField]
    private Script<object> compiledCode;
    //todo 5 reimplement some returns
    public object RunCode(Hardware hardware)
    {
        Debug.Log("start code");
        ScriptState<object> scriptState = null;
        try
        {
            var comp = compiledCode.GetCompilation();
            var entryPoint = comp.GetEntryPoint(CancellationToken.None);
            Debug.Log("entry:" + comp.GetEntryPoint(CancellationToken.None));
            Debug.Log($"{entryPoint.ReturnType} {entryPoint.Name}({string.Join(", ", entryPoint.Parameters.Select(p => $"{p.Type} {p.Name}"))})");
            //get all classes from compilation
            var classes = comp.SyntaxTrees.SelectMany(t => t.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>());

/*
            //log all of them

            Debug.Log("classes:" + string.Join(",", Array.ConvertAll(classes.ToArray(), x => x.Identifier.ToString())));
            //get classes that has a method
            var classesWithMethods = classes.Where(c => c.Members.OfType<MethodDeclarationSyntax>().Any());
            Debug.Log("classesWithMethods:" + string.Join(",", Array.ConvertAll(classesWithMethods.ToArray(), x => x.Identifier.ToString())));

            //get classes that has a method with main name
            var classesWithMainMethod = classesWithMethods.Where(c => c.Members.OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.ToString() == "Main"));
            Debug.Log("classesWithMainMethod:" + string.Join(",", Array.ConvertAll(classesWithMainMethod.ToArray(), x => x.Identifier.ToString())));

            //get classes that has a method with main name and no parameters
            var classesWithMainMethodNoParams = classesWithMainMethod.Where(c => c.Members.OfType<MethodDeclarationSyntax>().Any(m => m.ParameterList.Parameters.Count == 0));
            Debug.Log("classesWithMainMethodNoParams:" + string.Join(",", Array.ConvertAll(classesWithMainMethodNoParams.ToArray(), x => x.Identifier.ToString())));

            //get classes that has a method with main name and string[] parameters
            var classesWithMainMethodStringArrayParams = classesWithMainMethod.Where(c => c.Members.OfType<MethodDeclarationSyntax>().Any(m => m.ParameterList.Parameters.Count == 1 && m.ParameterList.Parameters[0].Type.ToString() == "string[]"));
            Debug.Log("classesWithMainMethodStringArrayParams:" + string.Join(",", Array.ConvertAll(classesWithMainMethodStringArrayParams.ToArray(), x => x.Identifier.ToString())));
            */
            //get classes that has static main method with string[] parameters OR no parameters
            var mainClasses = classes.Where(c => c.Members.OfType<MethodDeclarationSyntax>().Any(m => m.Identifier.ToString() == "Main" && m.Modifiers.Any(x => x.Text == "static") && (m.ParameterList.Parameters.Count == 0 || (m.ParameterList.Parameters.Count == 1 && m.ParameterList.Parameters[0].Type.ToString() == "string[]"))));






            /*  var mainClasses = classes
                  .Where(
                  c => c.Members.OfType<MethodDeclarationSyntax>()
                  .Any(m => m.Identifier.ToString() == "Main" &&
                  m.Modifiers.Any(mod => mod.Text == "static") &&
                  (m.ParameterList.Parameters.Count == 0 ||
                  (m.ParameterList.Parameters.Count == 1
                  && m.ParameterList.Parameters[0].Type.ToString() == "string[]"
                  ))));*/



            Debug.Log("mainClasses:" + string.Join(",", Array.ConvertAll(mainClasses.ToArray(), x => x.Identifier.ToString())));
            //log all of mainClasses in for loop
            foreach (var mainClass in mainClasses)
            {
                //get class name
                var className = mainClass.Identifier.ToString();
                //get main method
                Debug.Log("invoke: " + className + ".Main(*some args*)");
            }


            scriptState = compiledCode.RunAsync(hardware, catchException: HandleException);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        Debug.Log("end code");
        return scriptState?.ReturnValue;

    }
    bool HandleException(Exception exception)
    {
        //todo make handler
        Debug.Log(exception);
        return true;
    }
    public async void RunCodeAsync(Hardware hardware)
    {
        //todo 1 test
        await compiledCode.RunAsync(hardware);
    }
    public object RunCodeAsyncButReturn(Hardware hardware)
    {
        return Task.Run(() => { return RunCode(hardware); }).Result;
    }
    public Compilation(Script<object> script)
    {
        compiledCode = script;

    }
}
