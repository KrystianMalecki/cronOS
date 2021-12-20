using Microsoft.CodeAnalysis.CSharp.Syntax;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCLogic : MonoBehaviour
{
    public Hardware hardware;
    public bool isDefault = false;
    public static PCLogic defaultInstance;
    [Button, ExecuteInEditMode]
    public void SetDefault()
    {
        if (defaultInstance != null)
        {
            defaultInstance.isDefault = false;
        }
        defaultInstance = this;
        defaultInstance.isDefault = true;
    }
    public void Awake()
    {
        hardware.Init();

        if (isDefault)
        {
            SetDefault();
        }
    }
}
