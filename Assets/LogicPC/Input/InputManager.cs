using Libraries.system.input;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeReference] public HardwareInternal hardwareInternal;
    private object lockObj = new object();

    public StringBuilder inputBuffer = new StringBuilder();

    public ConcurrentHashSet<Key> currentlyPressedKeys = new();
    public ConcurrentHashSet<Key> currentlyPressedKeyBuffered = new();
    public ConcurrentHashSet<Key> _currentlyPressedKeyBuffered2 = new();

    public void Update()
    {
        if (hardwareInternal.focused)
        {
            if (Input.anyKey)
            {
                AddKeys();
                if (!string.IsNullOrEmpty(Input.inputString))
                {
                    lock (lockObj)
                    {
                        inputBuffer.Append(Input.inputString);
                    }
                }
            }
            else
            {
                lock (lockObj)
                {
                    if (currentlyPressedKeys.Count != 0)
                    {
                        currentlyPressedKeys.Clear();
                    }
                }
            }
        }
    }
    public void ClearInputBuffer()
    {
        DumpInputBuffer();
    }
    public void ClearStringInputBuffer()
    {
        inputBuffer.Clear();
    }
    //todo 9 check name
    public ConcurrentHashSet<Key> DumpInputBuffer()
    {
        lock (lockObj)
        {
            _currentlyPressedKeyBuffered2.Clear();
            _currentlyPressedKeyBuffered2.UnionWith(currentlyPressedKeyBuffered);
            currentlyPressedKeyBuffered.Clear();
            return _currentlyPressedKeyBuffered2;
        }
    }

    public void AddKeys()
    {
        lock (lockObj)
        {
            currentlyPressedKeys.Clear();
            currentlyPressedKeys.UnionWith(KeyboardInputHelper.GetCurrentKeysWrapped());
            currentlyPressedKeyBuffered.UnionWith(KeyboardInputHelper.GetCurrentKeysWrapped());
        }
    }

    public string GetInput()
    {
        if (!hardwareInternal.focused)
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