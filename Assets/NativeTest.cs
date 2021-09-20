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

    public void Update()
    {
        //  Debug.Log(String.Join(" , ", KeyboardInputHelper.GetCurrentKeys()));
        //  keysPressed.Add(KeyboardInputHelper.GetCurrentKeys());
    }
    public void OnDestroy()
    {


    }
    public IEnumerator ie()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Libraries.system.Input.cl();
        }
    }
}
