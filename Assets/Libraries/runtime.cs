//#define DLL

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace libraries.system
{
    public static class runtime
    {
        public static void Wait(int time)
        {
           // Task.Delay(time).Wait();
            Thread.Sleep(time);
        }
    }
}