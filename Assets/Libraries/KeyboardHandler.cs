
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static test;
using InternalLogger;
using System.Linq;


namespace Libraries.system
{

    public class KeyboardHandler
    {
        public HashSet<KeyboardKey> pressedDownKeys = new HashSet<KeyboardKey>();
        public HashSet<KeyboardKey> cooldownKeys = new HashSet<KeyboardKey>();
        //todo 1 fix or move it
        MainThreadDelegate<Exception> mtf;
        //maybe move to somewhere else
        public static KeyboardHandler Init()
        {
            KeyboardHandler kh = new KeyboardHandler();
            kh.StartRecordingInput();
            return kh;
        }
        KeyboardHandler()
        {
            _waitForInputAction = new MainThreadDelegate<KeyboardSequence>.MTDFunction((ref bool done, ref KeyboardSequence returnVal) =>
             {
                 RecalculatePressedKeys();
                 if (pressedDownKeys.Count > 0)
                 {

                     returnVal = new KeyboardSequence(pressedDownKeys.ToList());
                     done = true;
                 }
                 else
                 {
                     done = false;
                 }
             });
            _waitForInputDownAction = new MainThreadDelegate<KeyboardSequence>.MTDFunction((ref bool done, ref KeyboardSequence returnVal) =>
            {
                RecalculatePressedKeys();
                if (pressedDownKeys.Count > 0)
                {
                    cooldownKeys.UnionWith(pressedDownKeys);
                    returnVal = new KeyboardSequence(pressedDownKeys.ToList());
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
                IEnumerable<KeyboardKey> pressedNow = KeyboardInputHelper.GetCurrentKeysWrapped();
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

        public bool GetKeyDown(KeyboardKey key)
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
        ~KeyboardHandler()
        {
            InternalLogger.FlagLogger.Log(LogFlags.DebugInfo, "KeyboardHandler died.");
        }
        private MainThreadDelegate<KeyboardSequence>.MTDFunction _waitForInputAction;
        public KeyboardSequence WaitForInput()
        {
            return ScriptManager.AddDelegateToStack(_waitForInputAction, true);
        }
        private MainThreadDelegate<KeyboardSequence>.MTDFunction _waitForInputDownAction;

        public KeyboardSequence WaitForInputDown()
        {
            return ScriptManager.AddDelegateToStack(_waitForInputDownAction, true);
        }

    }
    public class KeyboardSequence
    {
        public List<KeyboardKey> keys = new List<KeyboardKey>();
        public bool HasKey(KeyboardKey key)
        {
            bool b = keys.Contains(key);
            keys.Remove(key);
            return b;
        }

        public KeyboardSequence(List<KeyboardKey> keys)
        {
            this.keys = keys;
        }
    }
}
