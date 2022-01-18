using System;

namespace Libraries.system.output.graphics
{
    namespace texture32
    {
        using Libraries.system.mathematics;
        using Libraries.system.output.graphics.color32;

        [Serializable]
        public class Texture32 : RectArray<Color32>
        {
            public Texture32(int width, int height) : base(width, height)
            {
            }

            public Texture32(RectArray<Color32> array) : base(array)
            {
            }

            public byte[] ToData()
            {
                return base.ToData(Color32.sizeOfInBits, x => new byte[4] { x.r, x.g, x.b, x.a });
            }

            public static Texture32 FromData(byte[] data)
            {
                return new Texture32(RectArray<Color32>.FromData(data, Color32.sizeOfInBits,
                    x => new Color32[] { new Color32(x[0], x[1], x[2], x[3]) }));
            }

            public void TintAll(Color32 color, bool addAlpha = true)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].Add(color, addAlpha);
                }
            }

            public bool UseTransparency()
            {
                return true;
            }
        }
    }
}