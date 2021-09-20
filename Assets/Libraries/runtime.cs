//#define DLL
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Libraries.system
{
    public class Runtime
    {
        public static void Wait(int time)
        {
            // Task.Delay(time).Wait();
            Thread.Sleep(time);
        }
    }

}