using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Util to help determine current keyboard keys pressed.
/// Does not support mouse and joystick key codes
/// </summary>
public static class KeyboardInputHelper
{
    public static readonly KeyCode[] _keyCodes =
        System.Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Where(k => k <= KeyCode.Mouse6)
            .ToArray();

    /// <summary>
    /// Get information about interesting keys pressed
    /// </summary>
    /// <param name="interestingCodes"></param>
    /// <returns>True if any of provided keys is down</returns>
    public static bool IsAnyKeyDown(params KeyCode[] interestingCodes)
    {
        return Enumerable.Intersect(GetCurrentKeys(), interestingCodes).Any();
    }

    /// <summary>
    /// Get information about interesting keys pressed
    /// </summary>
    /// <param name="interestingCodes">Interesting key codes</param>
    /// <returns>True if all of provided keys is down</returns>
    public static bool IsAllKeyDown(params KeyCode[] interestingCodes)
    {
        return Enumerable.SequenceEqual(GetCurrentKeys(), interestingCodes);
    }

    /// <summary>
    /// Get current keys pressed on keyboard including special keys like <see cref="KeyCode.LeftControl"/>
    /// </summary>
    /// <returns>Iterator with pressed keys. If nothing is pressed, empty iterator is returned</returns>
    /// <remarks>Be careful with FirstOrDefault. It will return KeyCode.None if nothing is pressed because of its implementation</remarks>
    public static IEnumerable<KeyCode> GetCurrentKeys()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < _keyCodes.Length; i++)
                if (Input.GetKey(_keyCodes[i]))
                    yield return _keyCodes[i];
        }
    }

    public static IEnumerable<Libraries.system.input.Key> GetCurrentKeysWrapped()
    {
        if (Input.anyKey)
        {
            for (int i = 0; i < _keyCodes.Length; i++)
                if (Input.GetKey(_keyCodes[i]))
                    yield return Libraries.system.input.KeyExtension.ToWrapper(_keyCodes[i]);
        }
    }

    public static IEnumerable<Libraries.system.input.Key> GetCurrentKeysUpWrapped()
    {
        for (int i = 0; i < _keyCodes.Length; i++)
            if (Input.GetKeyUp(_keyCodes[i]))
                yield return Libraries.system.input.KeyExtension.ToWrapper(_keyCodes[i]);
    }

    public static List<KeyCode> GetCurrentKeyss()
    {
        List<KeyCode> list = new List<KeyCode>();
        for (int i = 0; i < _keyCodes.Length; i++)
        {
            if (Input.GetKey(_keyCodes[i]))
            {
                list.Add(_keyCodes[i]);
            }
        }

        return list;
    }

    /// <summary>
    /// Get current keys unpressed on keyboard including special keys like <see cref="KeyCode.LeftControl"/>
    /// </summary>
    /// <returns>Iterator with unpressed keys. If nothing is pressed, empty iterator is returned</returns>
    /// <remarks>Be careful with FirstOrDefault. It will return KeyCode.None if nothing is pressed because of its implementation</remarks>
    public static IEnumerable<KeyCode> GetCurrentKeysUp()
    {
        for (int i = 0; i < _keyCodes.Length; i++)
            if (Input.GetKeyUp(_keyCodes[i]))
                yield return _keyCodes[i];
    }
}