using System.Collections;
using System.Collections.Generic;
using System.Text;
using Libraries.system.input;
using UnityEngine;
using System.Collections.Concurrent;

public class InputManager : MonoBehaviour
{
    [SerializeReference] public Hardware hardware;
    private object lockObj = new object();

    public StringBuilder inputBuffer = new StringBuilder();

    private ConcurrentHashSet<Key> keys = new();

    public void Update()
    {
        AddKeys();
        if (hardware.currentlySelected && Input.anyKey)
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

    public void AddKeys()
    {
        keys.UnionWith(KeyboardInputHelper.GetCurrentKeysWrapped());

        Debug.Log(keys.ToFormatedString());
    }

    public string GetInput()
    {
        if (!hardware.currentlySelected)
        {
            return string.Empty;
        }

        lock (lockObj)
        {
            string s = inputBuffer.ToString();
            inputBuffer.Clear();
            return s;
        }
    }
}