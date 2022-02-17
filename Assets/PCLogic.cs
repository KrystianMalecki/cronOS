using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PCLogic : MonoBehaviour
{
    public Hardware hardware;
    public static PCLogic defaultInstance;

    public void SetDefault(PCLogic logic)
    {
        if (defaultInstance != null)
        {
            defaultInstance.hardware.currentlySelected = false;
        }

        defaultInstance = logic;
        defaultInstance.hardware.currentlySelected = true;
    }

    [Button]
    void SetThisDefault()
    {
        SetDefault(this);
    }

    public void Awake()
    {
        hardware.Init();


        if (hardware.currentlySelected)
        {
            SetThisDefault();
        }
    }

    private void OnMouseEnter()
    {
        SetThisDefault();
    }



    private void OnMouseExit()
    {
        //  SetDefault(null);
    }
}