using System;

namespace Libraries.system.output.graphics
{ 
    namespace system_texture
    {
        using Libraries.system.mathematics;
        using Libraries.system.output.graphics.system_colorspace;

        [Serializable]
        public class SystemTexture : RectArray<SystemColor>
        {
            public SystemTexture(int width, int height) : base(width, height)
            {

            }
            public SystemTexture(RectArray<SystemColor> array) : base(array)
            {

            }
            public byte[] ToData()
            {
                return base.ToData(SystemColor.sizeOf, x => x.value.ToBytes());
            }

            public static SystemTexture FromData(byte[] data)
            {
                return new SystemTexture(RectArray<SystemColor>.FromData(data, SystemColor.sizeOf, x => x[0]));
            }
        
        }
    }
}
