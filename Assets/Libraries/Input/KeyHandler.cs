
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Libraries.system
{
    namespace input
    {
        public class KeyHandler
        {
            public HashSet<Key> pressedDownKeys = new HashSet<Key>();
            public HashSet<Key> cooldownKeys = new HashSet<Key>();
            private void CheckKeys()
            {
                try
                {
                    IEnumerable<Key> pressedNow = KeyboardInputHelper.GetCurrentKeysWrapped();
                    cooldownKeys.IntersectWith(pressedNow);
                    pressedDownKeys.Clear();
                    pressedDownKeys.UnionWith(pressedNow.Except(cooldownKeys));

                }
                catch (Exception e)
                {
                    Debug.Log(" error" + e);
                }
            }
            public bool GetKeyDown(Key key)
            {
                //test.instance.count5++;
                /*  if (pressedDownKeys.Contains(key))
                  {

                      pressedDownKeys.Remove(key);
                      cooldownKeys.Add(key);
                      ScriptManager.AddDelegateToStack(RecalculatePressedKeys, true);
                      return true;
                  }

                  return false;*/
                ScriptManager.AddDelegateToStack(CheckKeys, true);
                if (pressedDownKeys.Contains(key))
                {
                    pressedDownKeys.Remove(key);
                    cooldownKeys.Add(key);
                    ScriptManager.AddDelegateToStack(CheckKeys, true);
                    return true;
                }

                return false;
            }

            public KeySequence WaitForInput()
            {
                ScriptManager.AddDelegateToStack(CheckKeys, true);
                while (pressedDownKeys.Count <= 0)
                {
                    Runtime.Wait();
                    ScriptManager.AddDelegateToStack(CheckKeys, true);
                }
                return new KeySequence(pressedDownKeys);
            }
            public KeySequence WaitForInputDown()
            {
                ScriptManager.AddDelegateToStack(CheckKeys, true);

                while (pressedDownKeys.Count <= 0)
                {
                    Runtime.Wait();
                    ScriptManager.AddDelegateToStack(CheckKeys, true);
                }
                cooldownKeys.UnionWith(pressedDownKeys);
                return new KeySequence(pressedDownKeys);
            }
            public static string GetInputAsString()
            {
                return InputManager.GetInput(); //ScriptManager.AddDelegateToStack((ref bool done, ref string ret) => { ret = Input.inputString; }, true);
            }
            public static string WaitForStringInput()
            {
                string buffer = "";
                while (String.IsNullOrEmpty(buffer))
                {
                    buffer = InputManager.GetInput();
                    Runtime.Wait();
                }
                return buffer;

                /*  return ScriptManager.AddDelegateToStack((ref bool done, ref string ret) =>
                  {
                      done = false;
                      if (Input.anyKey)
                      {
                          ret = Input.inputString;
                          if (!String.IsNullOrEmpty(ret))
                          {
                              done = true;
                          }
                      }

                  }, true);*/
            }




            bool combinedCharacterMode = false;
            public string combinedBuffer = "";
            public string TryGetCombinedSymbol(KeySequence ks)
            {
                combinedCharacterMode = ks.ReadAnyAlt(false);
                if (combinedCharacterMode)
                {
                    int digit = ks.ReadDigit(true);
                    if (digit != -1)
                    {
                        Debug.Log("added"+digit+"to"+combinedBuffer);

                        combinedBuffer += digit;
                    }
                    if (combinedBuffer.Length == 3)
                    {
                        if (byte.TryParse(combinedBuffer, out byte value))
                        {
                            Debug.Log(value);

                            combinedBuffer = "";
                            combinedCharacterMode = false;
                            return "" + Runtime.ByteToChar(value);
                        }

                        combinedBuffer = "";
                        combinedCharacterMode = false;

                    }
                }
                else
                {
                    combinedBuffer = "";
                }
                return "";

            }
        }
        public class KeySequence
        {
            public List<Key> keys;
            public bool ReadKey(Key key, bool remove = true)
            {
                bool b = keys.Contains(key);
                if (remove)
                {
                    keys.Remove(key);
                }
                return b;
            }
            public KeySequence(List<Key> keys)
            {
                this.keys = new List<Key>(keys);
            }
            public KeySequence(IEnumerable<Key> keys)
            {
                this.keys = new List<Key>(keys);
            }
            public KeySequence(ThreadSafeList<Key> keys)
            {
                this.keys = new List<Key>(keys);
            }
            public override string ToString()
            {
                return String.Join(", ", keys);
            }
            public bool ReadAnyAlt(bool remove = true)
            {
                return ReadKey(Key.LeftAlt, remove) || ReadKey(Key.RightAlt, remove) || ReadKey(Key.AltGr, remove);
            }
            public bool ReadAnyShift(bool remove = true)
            {
                return ReadKey(Key.LeftShift, remove) || ReadKey(Key.RightShift, remove);

            }
            public bool ReadAnyCtrl(bool remove = true)
            {
                return ReadKey(Key.LeftControl, remove) || ReadKey(Key.RightControl, remove);
            }
            public int ReadDigit(bool remove = true)
            {
                if (ReadKey(Key.Alpha0, remove) || ReadKey(Key.Keypad0, remove))
                {
                    return 0;
                }
                else if (ReadKey(Key.Alpha1, remove) || ReadKey(Key.Keypad1, remove))
                {
                    return 1;
                }
                else if (ReadKey(Key.Alpha2, remove) || ReadKey(Key.Keypad2, remove))
                {
                    return 2;
                }
                else if (ReadKey(Key.Alpha3, remove) || ReadKey(Key.Keypad3, remove))
                {
                    return 3;
                }
                else if (ReadKey(Key.Alpha4, remove) || ReadKey(Key.Keypad4, remove))
                {
                    return 4;
                }
                else if (ReadKey(Key.Alpha5, remove) || ReadKey(Key.Keypad5, remove))
                {
                    return 5;
                }
                else if (ReadKey(Key.Alpha6, remove) || ReadKey(Key.Keypad6, remove))
                {
                    return 6;
                }
                else if (ReadKey(Key.Alpha7, remove) || ReadKey(Key.Keypad7, remove))
                {
                    return 7;
                }
                else if (ReadKey(Key.Alpha8, remove) || ReadKey(Key.Keypad8, remove))
                {
                    return 8;
                }
                else if (ReadKey(Key.Alpha9, remove) || ReadKey(Key.Keypad9, remove))
                {
                    return 9;
                }
                return -1;
            }
        }
        public static class InputHelpers
        {
            public static string AddInput(this string current, string input)
            {
                foreach (char c in input)
                {
                    if (c == '\b') // has backspace/delete been pressed?
                    {
                        if (current.Length != 0)
                        {
                            current = current.Substring(0, current.Length - 1);
                        }
                    }
                    else if ((c == '\n') || (c == '\r')) // enter/return
                    {
                        current += '\n';
                    }
                    else
                    {
                        current += c;
                    }
                }
                return current;
            }
            public static string AddInputSpecial(this string current, string input, KeySequence keySequence)
            {
                bool shift = keySequence.ReadAnyShift(false);
                bool alt = keySequence.ReadAnyAlt(false);
                bool ctrl = keySequence.ReadAnyCtrl(false);

                //Debug.Log($"alt{alt} shift{shift} ctrl{ctrl}");
                for (int i = 0; i < input.Length; i++)
                {
                    char c = input[i];
                    if (c == '\b') // has backspace/delete been pressed?
                    {
                        if (current.Length != 0)
                        {
                            current = current.Substring(0, current.Length - 1);
                            continue;
                        }
                    }
                    else if ((c == '\n') || (c == '\r')) // enter/return
                    {
                        current += '\n';
                        continue;
                    }
                    /* else if (alt)
                     {
                         //if (input.Length > (i + 1))
                         {
                             current += '\\' + c;
                             Debug.Log('\\' + c);
                             continue;


                         }
                     }*/


                    current += shift ? ("" + c).ToUpperInvariant() : c;

                }
                return current;
            }
        }
    }
}
