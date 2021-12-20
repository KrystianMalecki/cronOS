using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class InputManager : MonoBehaviour
{
    public bool currentlyEnabled = false;
    private object lockObj = new object();
   
    public StringBuilder inputBuffer = new StringBuilder();

    public void Update()
    {

        if (currentlyEnabled&&Input.anyKey)
        {
            if (!string.IsNullOrEmpty(Input.inputString))
            {

                lock (lockObj)
                {
                    inputBuffer.Append(Input.inputString);

                }

            }
        }
    }

    public string GetInput()
    {
        lock (lockObj)
        {
            string s = inputBuffer.ToString();
            inputBuffer.Clear();
            return s;

        }
    }
}
