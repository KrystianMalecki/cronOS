
using System;
using System.Collections;
using System.Collections.Generic;
using InternalLogger;
using System.Linq;

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
                    FlagLogger.Log(LogFlags.SystemError, " error", e);
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
                return InputManager2.GetInput(); //ScriptManager.AddDelegateToStack((ref bool done, ref string ret) => { ret = Input.inputString; }, true);
            }
            public static string WaitForStringInput()
            {
                string buffer = "";
                while (String.IsNullOrEmpty(buffer))
                {
                    buffer = InputManager2.GetInput();
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
        }
        public class KeySequence
        {
            public List<Key> keys;
            public bool HasKey(Key key)
            {
                bool b = keys.Contains(key);
                keys.Remove(key);
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
            /* public char GetFirstCharacter()
             {
                 Char.
                 Key k = keys.Find(x => Key.A >= x && x <= Key.Z);
             }*/
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
        }
    }
}
