using System;
using System.Linq;

namespace Libraries.system.output.graphics
{
    namespace mask_texture
    {
        using Libraries.system.mathematics;
        using System.Collections;
        using System.ComponentModel;
        using UnityEngine;

        [Serializable]
        public class MaskTexture : RectArray<bool> //todo 0 convert to palette texture with "static" palette
        {
            private static readonly int HEADER_SIZE = 1;
            public byte transparencyFlag = 0xff;

            public MaskTexture(int width, int height) : base(width, height)
            {
            }

            public MaskTexture(RectArray<bool> array) : base(array)
            {
            }

            public MaskTexture()
            {
            }

            public bool UseTransparency()
            {
                return transparencyFlag == 0xff;
            }

            public byte[] ToData()
            {
                byte[] header = new byte[HEADER_SIZE];

                header[0] = transparencyFlag;

                return header.Concat(base.ToData(sizeOfTInBits, Converter)).ToArray();
            }

            static byte sizeOfTInBits = 1;
            byte[] buffer = new byte[1];

            byte counter;
            BitArray ba = new BitArray(8, false);
            int index = 0;

            public byte[] Converter(bool x)
            {
                ba.Set(index++, x);
                if (index >= 8)
                {
                    ba.CopyTo(buffer, 0);
                    index = 0;
                    return buffer;
                }

                return Array.Empty<byte>();
            }

            public static MaskTexture FromData(byte[] data)
            {
                MaskTexture texture = new MaskTexture(RectArray<bool>.FromData(data.Skip(HEADER_SIZE).ToArray(),
                    sizeOfTInBits, x =>
                    {
                        bool[] bs = new bool[8];
                        BitArray ba = new BitArray(x);
                        ba.CopyTo(bs, 0);
                        return bs;
                    }));
                byte[] header = data.Take(HEADER_SIZE).ToArray();

                texture.transparencyFlag = header[0];

                return texture;
            }
        }
    }
}