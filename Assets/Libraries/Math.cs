
namespace Libraries.system
{
    namespace math
    {
        public struct Vector2
        {
            public float x;
            public float y;
            internal UnityEngine.Vector2 ToUnity()
            {
                return new UnityEngine.Vector2(x, y);
            }

            public Vector2(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
        }
        public struct Vector2Int
        {
            public int x;
            public int y;
            internal UnityEngine.Vector2Int ToUnity()
            {
                return new UnityEngine.Vector2Int(x, y);
            }

            public Vector2Int(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public Vector2Int((int x, int y) p)
            {
                this.x = p.x;
                this.y = p.y;
            }
        }
    }
}