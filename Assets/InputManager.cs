using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Libraries.system;
using InternalLogger;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public HashSet<KeyboardKey> pressedDownKeys = new HashSet<KeyboardKey>();
    public HashSet<KeyboardKey> cooldownKeys = new HashSet<KeyboardKey>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void RecalculatePressedKeys()
    {
        try
        {

            IEnumerable<KeyboardKey> pressedNow = KeyboardInputHelper.GetCurrentKeysWrapped();


            pressedDownKeys.UnionWith(pressedNow.Except(cooldownKeys)); //ass

            //remove cooldowned



            cooldownKeys.IntersectWith(pressedNow); //remove not pressed


            // Debug.Log(pressedDownKeys.ToArrayString() + cooldownKeys.ToArrayString());
            //  FlagLogger.Log(LogFlags.DebugInfo, pressedDownKeys.ToArrayString());
        }
        catch (Exception e)
        {
            //todo add error catch
            FlagLogger.Log(LogFlags.SystemError, " error", e);


        }
    }
    public void Update()
    {
        RecalculatePressedKeys();
    }
    
}
