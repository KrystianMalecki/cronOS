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

    public void SetDefault(PCLogic logic)
    {
        if (PlayerController.selectedPC != null)
        {
            PlayerController.selectedPC.hardware.currentlySelected = false;
        }

        PlayerController.selectedPC = logic;
        PlayerController.selectedPC.hardware.currentlySelected = true;
    }

    [Button]
    void SetThisDefault()
    {
        SetDefault(this);
    }

    public void Awake()
    {
        hardware.Init();



    }

    private void OnMouseEnter()
    {
        SetThisDefault();
        Debug.Log("Mouse entered");
    }



    private void OnMouseExit()
    {
        //  SetDefault(null);
    }
}