using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;


[System.AttributeUsage(System.AttributeTargets.Class |
                     System.AttributeTargets.Struct)
]
public class StructureAttribute : System.Attribute
{
    public string name;
    
   
}

