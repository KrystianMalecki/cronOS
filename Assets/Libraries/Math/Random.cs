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
        public class Random
        {

            public int seed;
            private System.Random systemRandom;

            public Random(int seed)
            {
                this.seed = seed;
                this.systemRandom = new System.Random(seed);
            }
            public Random()
            {
                this.systemRandom = new System.Random();
            }
            public int NextInt(int min = int.MinValue, int max = int.MaxValue)
            {
                return systemRandom.Next(min, max);
            }
            public byte NextByte(byte min = byte.MinValue, byte max = byte.MaxValue)
            {
                return (byte)NextInt(min, max);
            }
            public float NextFloat(float min = float.MinValue, float max = float.MaxValue)
            {
                return (float)NextDouble(min, max);
            }
            public double NextDouble(double min = double.MinValue, double max = double.MaxValue)
            {
                return (systemRandom.NextDouble() * max) - min;
            }
            public float NextFloat01()
            {
                return (float)NextDouble(0, 1);
            }
        }
    }
}