using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)
]
public class FieldAttribute : System.Attribute
{
    public string name;
    public string type = null;
    public string arraySize = null;
}
