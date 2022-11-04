using System;

using NaughtyAttributes;

using UnityEngine;
using System.Threading;
using Libraries.system.file_system;
using Libraries.system;

public class PCLogic : MonoBehaviour
{
    public HardwareInternal hardwareInternal = new HardwareInternal();


    private void SetDefault(PCLogic logic)
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
    private void Awake()
    {
        if (hardwareInternal.focused)
        {
            SetDefault(this);
        }
        Init();
    }
    public void Init()
    {
        hardwareInternal.Init();

    }
    //todo 3 remove
    private void Start()
    {
        // StartHardware();
    }
    [Button]
    public void StartHardware()
    {
        hardwareInternal.SystemInit();
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