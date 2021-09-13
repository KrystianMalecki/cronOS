//#define DLL

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace libraries.system
{
    public class random
    {

        public int seed;
        private System.Random systemRandom;

        public random(int seed)
        {
            this.seed = seed;
            this.systemRandom = new System.Random(seed);
        }
        public random()
        {
            this.systemRandom = new System.Random();
        }
        public int NextInt(int min = int.MinValue, int max = int.MaxValue)
        {
            return systemRandom.Next(min, max);
        }
        public byte NextByte(byte min = byte.MinValue, byte max = byte.MaxValue)
        {
            return (byte)systemRandom.Next(min, max);
        }
    }
}