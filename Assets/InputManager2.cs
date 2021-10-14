using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//todo 1 remove 2 when unity fixes random null file bug
public class InputManager2 : MonoBehaviour
{
    public static InputManager2 instance;
    private object lockObj = new object();
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public StringBuilder inputBuffer = new StringBuilder();

    public void Update()
    {

        if (Input.anyKey)
        {
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                lock (instance.lockObj)
                {
                    inputBuffer.Append(Input.inputString);
                }

            }
        }
    }

    public static string GetInput()
    {
        lock (instance.lockObj)
        {
            string s = instance.inputBuffer.ToString();
            instance.inputBuffer.Clear();
            return s;

        }
    }
}
