using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
/*
 <summary>
this is made from file.
</summary>
 */
[Serializable]
public class CodeObject
{
    [ResizableTextArea] public string code;
    public List<LibraryData> libraries;

    public CodeObject(string code, List<LibraryData> libraries)
    {
        this.code = code;
        this.libraries = libraries;
    }

    public CodeObject(string code, List<Type> libraryTypes)
    {
        this.code = code;
        this.libraries = libraryTypes.ConvertAll(x => x.ToLibraryData());
    }

    public void DisplayLibraries()
    {
        libraries.ForEach(x => { Debug.Log(x.assembly + "-" + x.nameSpace); });
    }
}

[SerializeField]
public class LibraryData
{
    public string assembly;
    public string nameSpace;

    public LibraryData(string assembly, string nameSpace)
    {
        this.assembly = assembly;
        this.nameSpace = nameSpace;
    }

    public override string ToString()
    {
        return "as:" + assembly + ", ns:" + nameSpace;
    }
}