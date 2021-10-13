
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static test;
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
    }
}
