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
                get { return v2.x; }
                set { v2.x = value; }
            }

            public float y
            {
                get { return v2.y; }
                set { v2.y = value; }
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

            public Vector2Int ToVector2Int()
            {
                return new Vector2Int((int)v2.x, (int)v2.y);
            }

            public static Vector2 zero = new Vector2(0, 0);
            public static Vector2 incorrectVector = new Vector2(float.MaxValue, float.MinValue);

            public override string ToString()
            {
                return v2.ToString();
            }

            //oerpator +
            public static Vector2 operator +(Vector2 a, Vector2 b)
            {
                return new Vector2(a.x + b.x, a.y + b.y);
            }

            //oerpator -
            public static Vector2 operator -(Vector2 a, Vector2 b)
            {
                return new Vector2(a.x - b.x, a.y - b.y);
            }

            //oerpator *
            public static Vector2 operator *(Vector2 a, Vector2 b)
            {
                return new Vector2(a.x * b.x, a.y * b.y);
            }

            //oerpator /
            public static Vector2 operator /(Vector2 a, Vector2 b)
            {
                return new Vector2(a.x / b.x, a.y / b.y);
            }
        }

        public struct Vector2Int
        {
            public UnityEngine.Vector2Int v2;

            public int x
            {
                get { return v2.x; }
                set { v2.x = value; }
            }

            public int y
            {
                get { return v2.y; }
                set { v2.y = value; }
            }

            public void SetX(int value)
            {
                x = value;
            }

            public void SetY(int value)

            {
                y = value;
            }

            public Vector2Int(int x, int y)
            {
                v2 = new UnityEngine.Vector2Int(x, y);
            }

            public Vector2 ToVector2()
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

            public static readonly Vector2Int zero = new Vector2Int(0, 0);

            public static bool operator ==(Vector2Int v1, Vector2Int v2)
            {
                return v1.x == v2.x && v1.y == v2.y;
            }

            public static bool operator !=(Vector2Int v1, Vector2Int v2)
            {
                return !(v1 == v2);
            }

            public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
            {
                return new Vector2Int(v1.x + v2.x, v1.y + v2.y);
            }

            //operator -
            public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
            {
                return new Vector2Int(v1.x - v2.x, v1.y - v2.y);
            }

            //operator *
            public static Vector2Int operator *(Vector2Int v1, Vector2Int v2)
            {
                return new Vector2Int(v1.x * v2.x, v1.y * v2.y);
            }

            //operator /
            public static Vector2Int operator /(Vector2Int v1, Vector2Int v2)
            {
                return new Vector2Int(v1.x / v2.x, v1.y / v2.y);
            }

            public override string ToString()
            {
                return v2.ToString();
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