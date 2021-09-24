using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NativeTest : MonoBehaviour
{
    Keyboard keyboard;
    HashSet<KeyCode> keysPressed;
    public void Start()
    {
       // StartCoroutine(ie());


    }

   /* public void Update()
    {
        //  Debug.Log(String.Join(" , ", KeyboardInputHelper.GetCurrentKeys()));
        //  keysPressed.Add(KeyboardInputHelper.GetCurrentKeys());
    }*/
    public void OnDestroy()
    {
     

    }
   
}
