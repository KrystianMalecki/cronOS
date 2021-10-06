
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static test;
using InternalLogger;
using System.Linq;


namespace Libraries.system
{

    public class KeyHandler
    {
        public HashSet<Key> pressedDownKeys = new HashSet<Key>();
        public HashSet<Key> cooldownKeys = new HashSet<Key>();
        //todo 1 fix or move it
        MainThreadDelegate<Exception> mtf;
        //maybe move to somewhere else
        public static KeyHandler Init()
        {
            KeyHandler kh = new KeyHandler();
            kh.StartRecordingInput();
            return kh;
        }
        KeyHandler()
        {
            _waitForInputAction = new MainThreadDelegate<KeySequence>.MTDFunction((ref bool done, ref KeySequence returnVal) =>
             {
                 RecalculatePressedKeys();
                 if (pressedDownKeys.Count > 0)
                 {

                     returnVal = new KeySequence(pressedDownKeys.ToList());
                     done = true;
                 }
                 else
                 {
                     done = false;
                 }
             });
            _waitForInputDownAction = new MainThreadDelegate<KeySequence>.MTDFunction((ref bool done, ref KeySequence returnVal) =>
            {
                RecalculatePressedKeys();
                if (pressedDownKeys.Count > 0)
                {
                    cooldownKeys.UnionWith(pressedDownKeys);
                    returnVal = new KeySequence(pressedDownKeys.ToList());
                    done = true;
                }
                else
                {
                    done = false;
                }
            });
        }
        public void StartRecordingInput()

        {
            mtf = new MainThreadDelegate<Exception>((ref bool done, ref Exception returnVal) =>
            {
                done = false;
                RecalculatePressedKeys();
                done = false;
            });
            mtf.Speak();

            ScriptManager.AddDelegateToStack(mtf, false);
        }
        public void RecalculatePressedKeys()
        {
            try
            {
                IEnumerable<Key> pressedNow = KeyboardInputHelper.GetCurrentKeysWrapped();
                pressedDownKeys.Clear();
                pressedDownKeys.UnionWith(pressedNow.Except(cooldownKeys)); //ass

                //remove cooldowned
                cooldownKeys.IntersectWith(pressedNow); //remove not pressed
            }
            catch (Exception e)
            {
                FlagLogger.Log(LogFlags.SystemError, " error", e);
            }
        }

        public bool GetKeyDown(Key key)
        {
            //test.instance.count5++;
            if (pressedDownKeys.Contains(key))
            {

                pressedDownKeys.Remove(key);
                cooldownKeys.Add(key);
                ScriptManager.AddDelegateToStack(RecalculatePressedKeys, true);
                return true;
            }

            return false;
        }
        ~KeyHandler()
        {
            InternalLogger.FlagLogger.Log(LogFlags.DebugInfo, "KeyboardHandler died.");
        }
        private MainThreadDelegate<KeySequence>.MTDFunction _waitForInputAction;
        public KeySequence WaitForInput()
        {
            return ScriptManager.AddDelegateToStack(_waitForInputAction, true);
        }
        private MainThreadDelegate<KeySequence>.MTDFunction _waitForInputDownAction;

        public KeySequence WaitForInputDown()
        {
            return ScriptManager.AddDelegateToStack(_waitForInputDownAction, true);
        }

    }
    public class KeySequence
    {
        public List<Key> keys = new List<Key>();
        public bool HasKey(Key key)
        {
            bool b = keys.Contains(key);
            keys.Remove(key);
            return b;
        }

        public KeySequence(List<Key> keys)
        {
            this.keys = keys;
        }
    }
}
