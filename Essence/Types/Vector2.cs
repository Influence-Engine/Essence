using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Essence
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public float x;
        public float y;

        public float this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index == 0) return x;
                else if (index == 1) return y;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index == 0) x = value;
                else if (index == 1) y = value;
                else throw new IndexOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float x, float y)
        {
            this.x = x; 
            this.y = y;
        }

        #region Functions

        public float Magnitude => (float)MathF.Sqrt(x * x + y * y);

        public float SqrMagnitude => x * x + y * y;

        #region Normalization

        public Vector2 Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Vector2 v = new Vector2(x, y);
                v.Normalize();
                return v;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            float magnitude = Magnitude;
            if(magnitude > 0f)
            {
                this.x /= magnitude;
                this.y /= magnitude;
            }
            else
            {
                this.x = 0f;
                this.y = 0f;
            }
        }

        #endregion

        #endregion

        #region 

        #region Common Overrides (ToString, GetHashCode, Equals)

        public override string ToString() => $"({x}, {y})";

        public override bool Equals(object? other)
        {
            if (!(other is Vector2))
                return false;

            return Equals((Vector2)other);
        }

        public bool Equals(Vector2 other) => x == other.x && y == other.y;

        public override int GetHashCode() => x.GetHashCode() ^ (y.GetHashCode() * 4);

        #endregion
    }
}
