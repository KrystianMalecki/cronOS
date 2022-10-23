using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PCLogic : MonoBehaviour
{
    public HardwareInternal hardwareInternal = new HardwareInternal();

    public void SetDefault(PCLogic logic)
    {
        if (PlayerController.selectedPC != null)
        {
            PlayerController.selectedPC.hardwareInternal.focused = false;
        }

        PlayerController.selectedPC = logic;
        PlayerController.selectedPC.hardwareInternal.focused = true;
    }

    [Button]
    void SetThisDefault()
    {
        SetDefault(this);
    }

    public void Awake()
    {
        hardwareInternal.Init();
        if (hardwareInternal.focused)
        {
            SetDefault(this);
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