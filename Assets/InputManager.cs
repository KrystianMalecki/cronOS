using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class InputManager : MonoBehaviour
{
    public static InputManager instance;
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
     //   StartCoroutine(ie());
    }
    public IEnumerator ie()
    {
        while (true)
        {
            currentlyPressed = KeyboardInputHelper.GetCurrentKeyss();
            Debug.Log(string.Join(" ", currentlyPressed));

            yield return new WaitForSeconds(1f);
        }
    }
    public List<KeyCode> currentlyPressed = new List<KeyCode>();
     
   
}
