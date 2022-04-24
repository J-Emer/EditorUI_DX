using System;
using Microsoft.Xna.Framework;


namespace EditorUI_DX.Utils
{
    public struct Vector2_Int : IEquatable<Vector2_Int>
    {
        public int X;
        public int Y;


        private static readonly Vector2_Int zeroVector = new Vector2_Int(0, 0);
        private static readonly Vector2_Int unitVector = new Vector2_Int(1, 1);
        private static readonly Vector2_Int unitXVector = new Vector2_Int(1, 0);
        private static readonly Vector2_Int unitYVector = new Vector2_Int(0, 1);

        public static Vector2_Int Zero
        {
            get
            {
                return zeroVector;
            }
        }
        public static Vector2_Int One
        {
            get
            {
                return unitVector;
            }
        }
        public static Vector2_Int UnitX
        {
            get
            {
                return unitXVector;
            }
        }
        public static Vector2_Int UnitY
        {
            get
            {
                return unitYVector;
            }
        }

        public Vector2_Int(int _x, int _y)
        {
            this.X = _x;
            this.Y = _y;
        }

        /// <summary>
        /// Inverts values in the specified <see cref="Vector2_Int"/>.
        /// </summary>
        public static Vector2_Int operator -(Vector2_Int value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }


        /// <summary>
        /// Adds two Vector2_Ints
        /// </summary>
        public static Vector2_Int operator +(Vector2_Int value1, Vector2_Int value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Subtracts two Vector2_Ints
        /// </summary>
        public static Vector2_Int operator -(Vector2_Int value1, Vector2_Int value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies two Vector2_Ints
        /// </summary>
        public static Vector2_Int operator *(Vector2_Int value1, Vector2_Int value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies a Vector2_Int by a scale factor
        /// </summary>
        public static Vector2_Int operator *(Vector2_Int value1, int scale)
        {
            value1.X += scale;
            value1.Y += scale;
            return value1;
        }
        public static Vector2_Int operator /(Vector2_Int value1, int scale)
        {
            value1.X /= scale;
            value1.Y /= scale;
            return value1;
        }
        public static Vector2_Int operator /(Vector2_Int value1, Vector2_Int value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        public static bool operator ==(Vector2_Int value1, Vector2_Int value2)
        {
            return (value1.X == value2.X && value1.Y == value2.Y);
        }
        public static bool operator !=(Vector2_Int value1, Vector2_Int value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        public static Vector2 ToVec2(Vector2_Int _value)
        {
            return new Vector2(_value.X, _value.Y);
        }
        public static Vector2_Int FromVec2(Vector2 _value)
        {
            return new Vector2_Int((int)_value.X, (int)_value.Y);
        }



        public override string ToString()
        {
            return $"{X} {Y}";
        }
        public override int GetHashCode()
        {
            return (X.GetHashCode() * 397) ^ Y.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Vector2_Int)
            {
                return Equals((Vector2_Int)obj);
            }
            return false;
        }
        public bool Equals(Vector2_Int other)
        {
            return (X == other.X) && (Y == other.Y);
        }
    }
}
