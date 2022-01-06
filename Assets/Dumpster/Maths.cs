using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;


namespace Libraries.system
{
    namespace mathematics
    {
        public static class Maths //todo 3 change name
        {
           //todo 5, think if using span would be better

            #region bool->byte

            public static byte Convert8BoolsToByte(this bool[] array)
            {
                byte output = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (array.Length > i && array[i])
                    {
                        output |= (byte)(1 << i);
                    }
                }

                return output;
            }

            public static byte[] ConvertAllBoolsToBytes(this bool[] array)
            {
                byte[] bytes = new byte[Math.Ceil(1f * array.Length / 8f)];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert8BoolsToByte(array.Skip(i * 8).Take(8).ToArray());
                }

                return bytes;
            }

            #endregion

            #region byte->bool

            public static bool[] ConvertBytesToBools(this byte[] array)
            {
                bool[] bools = new bool[array.Length * 8];
                for (int i = 0; i < array.Length; i++)
                {
                    Array.Copy(ConvertByteTo8Bools(array[i]), 0, bools, i * 8, 8);
                }

                return bools;
            }


            public static bool[] ConvertByteTo8Bools(this byte value)
            {
                bool[] output = new bool[8];
                for (int i = 0; i < 8; i++)
                {
                    output[i] = (value & (1 << i)) != 0;
                }

                return output;
            }

            #endregion

            #region nibble->byte

            public static byte Convert2NibblesToByte(this byte[] array)
            {
                byte output = 0;
                for (int i = 0; i < 2; i++)
                {
                    byte nibble = 0;
                    if (array.Length > i)
                    {
                        nibble = array[i];
                    }

                    output |= (byte)(nibble << (4 * i));
                }

                return output;
            }

            public static byte[] ConvertAllNibblesToBytes(this byte[] array)
            {
                byte[] bytes = new byte[Math.Ceil(1f * array.Length / 2f)];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert2NibblesToByte(array.Skip(i * 2).Take(2).ToArray());
                }

                return bytes;
            }

            #endregion

            #region byte->nibble

            public static byte[] ConvertByteTo2Nibbles(this byte value)
            {
                byte[] output = new byte[2];
                for (int i = 0; i < 2; i++)
                {
                    output[i] = (byte)((value >> (4 * i)) & 0x0F);
                }

                return output;
            }

            public static byte[] ConvertBytesToNibbles(this byte[] array)
            {
                byte[] nibbles = new byte[array.Length * 2];
                for (int i = 0; i < array.Length; i++)
                {
                    Array.Copy(ConvertByteTo2Nibbles(array[i]), 0, nibbles, i * 2, 2);
                }

                return nibbles;
            }

            #endregion

            #region dibit->byte

            public static byte Convert4DibitsToByte(this byte[] array)
            {
                byte output = 0;
                for (int i = 0; i < 4; i++)
                {
                    byte dibit = 0;
                    if (array.Length > i)
                    {
                        dibit = array[i];
                    }

                    output |= (byte)(dibit << (2 * i));
                }

                return output;
            }

            public static byte[] ConvertAllDibitsToBytes(this byte[] array)
            {
                byte[] bytes = new byte[Math.Ceil(1f * array.Length / 4f)];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert4DibitsToByte(array.Skip(i * 4).Take(4).ToArray());
                }

                return bytes;
            }

            #endregion

            #region byte->dibit

            public static byte[] ConvertByteTo4Dibits(this byte value)
            {
                byte[] output = new byte[4];
                output[0] = (byte)(value & 0b0000_0011);
                output[1] = (byte)(value >> 2 & 0b0000_0011);
                output[2] = (byte)(value >> 4 & 0b0000_0011);
                output[3] = (byte)(value >> 6 & 0b0000_0011);


                return output;
            }

            public static byte[] ConvertBytesToDibits(this byte[] array)
            {
                byte[] dibits = new byte[array.Length * 4];
                for (int i = 0; i < array.Length; i++)
                {
                    Array.Copy(ConvertByteTo4Dibits(array[i]), 0, dibits, i * 4, 4);
                }

                return dibits;
            }

            #endregion
        }
    }
}

public interface Convertable<T>
{
    void Convert(T t, Span<byte> bytes);
}