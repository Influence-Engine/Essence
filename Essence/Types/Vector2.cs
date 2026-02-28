using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Essence
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vector2 : IEquatable<Vector2>, IFormattable
    {
        public float x;
        public float y;

        public float this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= 2)
                    throw new IndexOutOfRangeException();

                return Unsafe.Add(ref x, index);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if ((uint)index >= 2)
                    throw new IndexOutOfRangeException();

                Unsafe.Add(ref x, index) = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float x, float y)
        {
            this.x = x; 
            this.y = y;
        }

        #region Functions

        public float Magnitude => MathF.Sqrt(x * x + y * y);

        public float SqrMagnitude => x * x + y * y;

        #region Normalization

        public Vector2 Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                float mag = MathF.Sqrt(x * x + y * y);
                if(mag > 0f)
                    return new Vector2(x / mag, y / mag);

                return default;
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

        #region Common Overrides (ToString, GetHashCode, Equals)

        public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);

        public readonly string ToString(string? format, IFormatProvider? formatProvider = null)
        {
            string fmt = format ?? "G";
            IFormatProvider provider = formatProvider ?? CultureInfo.CurrentCulture;
            return $"({x.ToString(fmt, provider)}, {y.ToString(fmt, provider)})";
        }

        public readonly override bool Equals(object? other)
        {
            if (!(other is Vector2))
                return false;

            return Equals((Vector2)other);
        }

        public readonly bool Equals(Vector2 other) => x == other.x && y == other.y;

        public readonly override int GetHashCode() => HashCode.Combine(x, y);

        #endregion
    }
}
