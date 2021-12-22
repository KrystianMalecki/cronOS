using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Libraries.system
{
    namespace input
    {
        public class KeyHandler : BaseLibrary
        {
            public ConcurrentHashSet<Key> pressedDownKeys = new ConcurrentHashSet<Key>();

            public ConcurrentHashSet<Key> cooldownKeys = new ConcurrentHashSet<Key>();

            // public static MainThreadDelegate<bool>.MTDFunction waitForcheckKeysDelegate = null;
            // public static MainThreadDelegate<bool>.MTDFunction justCheckKeysDelegate = null;
            public object locker = new();

            public override void Init(Hardware hardware)
            {
                base.Init(hardware);
                //  waitForcheckKeysDelegate = WaitForCheckKeysChange;
                //  justCheckKeysDelegate = JustCheckKeys;
            }

            /* private bool CheckKeys()
             {
                 if (!hardware.currentlySelected)
                 {
                     return false;
                 }
 
                 try
                 {
                     IEnumerable<Key> pressedNow = KeyboardInputHelper.GetCurrentKeysWrapped();
                     cooldownKeys.IntersectWith(pressedNow);
                     pressedDownKeys.Clear();
                     pressedDownKeys.UnionWith(pressedNow.Except(cooldownKeys));
                     return true;
                 }
                 catch (Exception e)
                 {
                     Debug.Log(" error" + e);
                     return false;
                 }
 
                 return true;
             }*/

            private void AddKeys(ConcurrentHashSet<Key> pressedNow)
            {
                lock (locker)
                {
                    try
                    {
                        cooldownKeys.IntersectWith(pressedNow);
                        pressedDownKeys.Clear();


                        pressedDownKeys.UnionWith(pressedNow.Except(cooldownKeys));
                    }
                    catch (Exception)
                    {
                        // no error here :)
                    }
                }
            }

            private void AddKeysFromInput()
            {
                lock (locker)
                {
                    AddKeys(hardware.hardwareInternal.inputManager.currentlyPressedKeys);
                }
            }

            /*  private void WaitForCheckKeysChange(ref bool done, ref bool result)
              {
                  done = false;
                  if (CheckKeys())
                  {
                      done = true;
                  }
              }
  
              private void JustCheckKeys(ref bool done, ref bool result)
              {
                  CheckKeys();
                  done = true;
              }*/

            public bool GetKeyDown(Key key)
            {
                if (!hardware.currentlySelected)
                {
                    return false;
                }

                //test.instance.count5++;
                /*  if (pressedDownKeys.Contains(key))
                  {

                      pressedDownKeys.Remove(key);
                      cooldownKeys.Add(key);
                      ScriptManager.AddDelegateToStack(RecalculatePressedKeys, true);
                      return true;
                  }

                  return false;*/
                // hardware.hardwareInternal.stackExecutor.AddDelegateToStack(justCheckKeysDelegate, true);
                AddKeysFromInput();
                if (pressedDownKeys.Contains(key))
                {
                    pressedDownKeys.Remove(key);
                    cooldownKeys.Add(key);
                    AddKeysFromInput();
                    return true;
                }

                return false;
            }

            public KeySequence WaitForInput()
            {
                do
                {
                    hardware.runtime.Wait();
                    AddKeysFromInput();
                } while (pressedDownKeys.Count <= 0);

                return new KeySequence(pressedDownKeys);
            }

            public KeySequence WaitForInputDown()
            {
                do
                {
                    hardware.runtime.Wait();
                    AddKeysFromInput();
                } while (pressedDownKeys.Count <= 0);

                cooldownKeys.UnionWith(pressedDownKeys);
                return new KeySequence(pressedDownKeys);
            }

            public string GetInputAsString()
            {
                return
                    hardware.hardwareInternal.inputManager
                        .GetInput();
            }

            public string WaitForStringInput()
            {
                string buffer = "";
                while (String.IsNullOrEmpty(buffer))
                {
                    buffer = hardware.hardwareInternal.inputManager.GetInput();
                    hardware.runtime.Wait();
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

            public string TryGetCombinedSymbol(ref KeySequence ks)
            {
                combinedCharacterMode = ks.ReadAnyAlt(true);
                if (combinedCharacterMode)
                {
                    int digit = ks.ReadDigit(out Key key, true);
                    if (digit != -1)
                    {
                        cooldownKeys.Add(key);


                        combinedBuffer += digit;
                    }

                    if (combinedBuffer.Length == 3)
                    {
                        if (byte.TryParse(combinedBuffer, out byte value))
                        {
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

            public int ReadDigit(out Key readDigit, bool remove = true)
            {
                readDigit = Key.None;
                if (ReadKey(Key.Alpha0, remove))
                {
                    readDigit = Key.Alpha0;
                    return 0;
                }
                else if (ReadKey(Key.Keypad0, remove))
                {
                    readDigit = Key.Keypad0;
                    return 0;
                }
                else if (ReadKey(Key.Alpha1, remove))
                {
                    readDigit = Key.Alpha1;
                    return 1;
                }
                else if (ReadKey(Key.Keypad1, remove))
                {
                    readDigit = Key.Keypad1;
                    return 1;
                }
                else if (ReadKey(Key.Alpha2, remove))
                {
                    readDigit = Key.Alpha2;
                    return 2;
                }
                else if (ReadKey(Key.Keypad2, remove))
                {
                    readDigit = Key.Keypad2;
                    return 2;
                }
                else if (ReadKey(Key.Alpha3, remove))
                {
                    readDigit = Key.Alpha3;
                    return 3;
                }
                else if (ReadKey(Key.Keypad3, remove))
                {
                    readDigit = Key.Keypad3;
                    return 3;
                }
                else if (ReadKey(Key.Alpha4, remove))
                {
                    readDigit = Key.Alpha4;
                    return 4;
                }
                else if (ReadKey(Key.Keypad4, remove))
                {
                    readDigit = Key.Keypad4;
                    return 4;
                }
                else if (ReadKey(Key.Alpha5, remove))
                {
                    readDigit = Key.Alpha5;
                    return 5;
                }
                else if (ReadKey(Key.Keypad5, remove))
                {
                    readDigit = Key.Keypad5;
                    return 5;
                }
                else if (ReadKey(Key.Alpha6, remove))
                {
                    readDigit = Key.Alpha6;
                    return 6;
                }
                else if (ReadKey(Key.Keypad6, remove))
                {
                    readDigit = Key.Keypad6;
                    return 6;
                }
                else if (ReadKey(Key.Alpha7, remove))
                {
                    readDigit = Key.Alpha7;
                    return 7;
                }
                else if (ReadKey(Key.Keypad7, remove))
                {
                    readDigit = Key.Keypad7;
                    return 7;
                }
                else if (ReadKey(Key.Alpha8, remove))
                {
                    readDigit = Key.Alpha8;
                    return 8;
                }
                else if (ReadKey(Key.Keypad8, remove))
                {
                    readDigit = Key.Keypad8;
                    return 8;
                }
                else if (ReadKey(Key.Alpha9, remove))
                {
                    readDigit = Key.Alpha9;
                    return 9;
                }
                else if (ReadKey(Key.Keypad9, remove))
                {
                    readDigit = Key.Keypad9;
                    return 9;
                }

                return -1;
            }

            public int ReadDigit(bool remove = true)
            {
                return ReadDigit(out _, remove);
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