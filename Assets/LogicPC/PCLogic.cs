
using NaughtyAttributes;

using UnityEngine;

public class PCLogic : MonoBehaviour
{
    public HardwareInternal hardwareInternal = new HardwareInternal();
    public static PCLogic selectedPC;

    private void SetDefault(PCLogic logic)
    {
        if (selectedPC != null)
        {
            selectedPC.hardwareInternal.focused = false;
        }

        selectedPC = logic;
        selectedPC.hardwareInternal.focused = true;
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