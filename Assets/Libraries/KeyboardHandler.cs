
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

        MainThreadDelegate<Exception> mtf;
        //maybe move to somewhere else
        public static KeyboardHandler Init()
        {
            KeyboardHandler kh = new KeyboardHandler();
            kh.StartRecordingInput();
            return kh;
        }
        public void StartRecordingInput()

        {
            mtf = new MainThreadDelegate<Exception>((ref bool done, ref Exception returnVal) =>
            {
                //  test.instance.count4++;
                done = false;
                //  Debug.Log("fuck you");
                RecalculatePressedKeys();
                done = false;
            });
            mtf.Speak();

            CodeRunner.AddFunctionToStack(mtf, false);
        }
        public void RecalculatePressedKeys()
        {
            try
            {

                IEnumerable<KeyboardKey> pressedNow = KeyboardInputHelper.GetCurrentKeysWrapped();


                pressedDownKeys.UnionWith(pressedNow.Except(cooldownKeys)); //ass

                //remove cooldowned



                cooldownKeys.IntersectWith(pressedNow); //remove not pressed


                // Debug.Log(pressedDownKeys.ToArrayString() + cooldownKeys.ToArrayString());
                //  FlagLogger.Log(LogFlags.DebugInfo, pressedDownKeys.ToArrayString());
            }
            catch (Exception e)
            {
                //todo add error catch
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
                CodeRunner.AddFunctionToStack(RecalculatePressedKeys, true);
                return true;
            }

            return false;
        }
        ~KeyboardHandler()
        {
            InternalLogger.FlagLogger.Log(LogFlags.DebugInfo, "died");
            Debug.Log("Co dop chuja kh");
        }
        public KeyboardSequence WaitForInputDown()
        {
            MainThreadDelegate<KeyboardSequence> mtd = new MainThreadDelegate<KeyboardSequence>((ref bool done, ref KeyboardSequence returnVal) =>
            {
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
            return CodeRunner.AddFunctionToStack(mtd, true);

        }
        public class KeyboardSequence
        {
            //todo maybe change
            public List<KeyboardKey> keys = new List<KeyboardKey>();
            public bool HasKey(KeyboardKey key)
            {
                return keys.Contains(key);
            }

            public KeyboardSequence(List<KeyboardKey> keys)
            {
                this.keys = keys;
            }
        }

    }
}
