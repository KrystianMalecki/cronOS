//#define DLL

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace Libraries.system
{
    namespace mathematics
    {
        public class Math
        {
            public static decimal Abs(decimal number)
            {
                return System.Math.Abs(number);
            }

            public static float Abs(float number)
            {
                return System.Math.Abs(number);
            }

            public static int Abs(int number)
            {
                return System.Math.Abs(number);
            }

            public static int Round(decimal number)
            {
                return (int)(number);
            }

            public static int Round(double number)
            {
                return (int)(number);
            }

            public static int Ceil(float number)
            {
                return UnityEngine.Mathf.CeilToInt(number);
            }

            public static int Round(float number)
            {
                return (int)(number);
            }
        }
    }
}