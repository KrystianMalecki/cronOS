
namespace Libraries.system
{
    namespace mathematics
    {
        /* public struct Vector2
         {
             public float x;
             public float y;
             internal UnityEngine.Vector2 ToUnity()
             {
                 return new UnityEngine.Vector2(x, y);
             }
             static internal Vector2 FromUnity(UnityEngine.Vector2 v2)
             {
                 return new Vector2(v2.x, v2.y);
             }
             public Vector2(float x, float y)
             {
                 this.x = x;
                 this.y = y;
             }
             public Vector2Int ToIntLike()
             {
                 return new Vector2Int((int)x, (int)y);
             }
         }*/
        public struct Vector2
        {
            public UnityEngine.Vector2 v2;
            public float x
            {
                get
                {
                    return v2.x;
                }
                set
                {
                    v2.x = value;
                }
            }
            public float y
            {
                get
                {
                    return v2.y;
                }
                set
                {
                    v2.y = value;
                }
            }
            public static implicit operator UnityEngine.Vector2(Vector2 vector2Based)
            {
                return vector2Based.v2;
            }
            public static implicit operator Vector2(UnityEngine.Vector2 vector2)
            {
                return new Vector2() { v2 = vector2 };
            }
            public Vector2(float x, float y)
            {
                v2.x = x;
                v2.y = y;
            }
            public Vector2Int ToIntLike()
            {
                return new Vector2Int((int)v2.x, (int)v2.y);
            }
        }
        public struct Vector2Int
        {
            public UnityEngine.Vector2Int v2;

            public int x
            {
                get
                {
                    return v2.x;
                }
                set
                {
                    v2.x = value;
                }
            }
            public int y
            {
                get
                {
                    return v2.y;
                }
                set
                {
                    v2.y = value;
                }
            }
            public Vector2Int(int x, int y)
            {
                v2 = new UnityEngine.Vector2Int(x, y);
            }

            public Vector2 ToFloatLike()
            {
                return new Vector2(v2.x, v2.y);
            }
            public Vector2Int((int x, int y) p) : this(p.x, p.y)
            {

            }
            public static implicit operator UnityEngine.Vector2Int(Vector2Int vector2IntBased)
            {
                return vector2IntBased.v2;
            }
            public static implicit operator Vector2Int(UnityEngine.Vector2Int vector2)
            {
                return new Vector2Int() { v2 = vector2 };
            }

        }
        /*    public struct Vector2Int
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
                static internal Vector2Int FromUnity(UnityEngine.Vector2Int v2)
                {
                    return new Vector2Int(v2.x, v2.y);
                }
                public Vector2 ToFloatLike()
                {
                    return new Vector2(x, y);
                }
                public Vector2Int((int x, int y) p)
                {
                    this.x = p.x;
                    this.y = p.y;
                }
            }*/
    }
}