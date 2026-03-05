using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Essence
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vector2Int : IEquatable<Vector2Int>, IFormattable
    {
        public int x;
        public int y;

        public int this[int index]
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

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2Int(int value)
        {
            this.x = value;
            this.y = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2Int(int x, int y)
        {
            this.x = x; 
            this.y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2Int(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2Int(Vector2 v)
        {
            this.x = (int)v.x;
            this.y = (int)v.y;
        }

        #endregion

        #region Common Static Vectors

        public static readonly Vector2Int Zero = new Vector2Int(0, 0);
        public static readonly Vector2Int One = new Vector2Int(1, 1);

        public static readonly Vector2Int Up = new Vector2Int(0, 1);
        public static readonly Vector2Int Down = new Vector2Int(0, -1);
        public static readonly Vector2Int Left = new Vector2Int(-1, 0);
        public static readonly Vector2Int Right = new Vector2Int(1, 0);

        public static readonly Vector2Int MinValue = new(int.MinValue, int.MinValue);
        public static readonly Vector2Int MaxValue = new(int.MaxValue, int.MaxValue);

        public static readonly Vector2Int NegativeOne = new Vector2Int(-1, -1);

        #endregion

        #region Properties

        public readonly float Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MathF.Sqrt(x * x + y * y);
        }

        public readonly float SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => x * x + y * y;
        }

        public readonly Vector2 Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                float mag = MathF.Sqrt(x * x + y * y);
                return mag > 1e-6f ? new Vector2(x / mag, y / mag) : Vector2.Zero;
            }
        }

        public readonly Vector2Int Perpendicular
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Vector2Int(-y, x);
        }

        public readonly bool IsZero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => x == 0 && y == 0;
        }

        #endregion

        #region Instance Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out int outX, out int outY)
        {
            outX = x;
            outY = y;
        }

        #endregion

        #region Static Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Min(Vector2Int a, Vector2Int b) => new Vector2Int(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Max(Vector2Int a, Vector2Int b) => new Vector2Int(Math.Max(a.x, b.x), Math.Max(a.y, b.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Abs(Vector2Int a) => new Vector2Int(Math.Abs(a.x), Math.Abs(a.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Clamp(Vector2Int vector, Vector2Int min, Vector2Int max) => new Vector2Int(Math.Clamp(vector.x, min.x, max.x), Math.Clamp(vector.y, min.y, max.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Scale(Vector2Int a, Vector2Int b) => new Vector2Int(a.x * b.x, a.y * b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector2Int a, Vector2Int b) => a.x * b.x + a.y * b.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(Vector2Int a, Vector2Int b) => a.x * b.y - a.y * b.x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2Int a, Vector2Int b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SqrDistance(Vector2Int a, Vector2Int b)
        {
            int dx = a.x - b.x;
            int dy = a.y - b.y;
            return dx * dx + dy * dy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Reflect(Vector2 direction, Vector2 normal)
        {
            float dot2 = 2f * (direction.x * normal.x + direction.y * normal.y);
            return new Vector2Int(direction.x - dot2 * normal.x, direction.y - dot2 * normal.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Project(Vector2Int a, Vector2Int b)
        {
            float sqrMagnitude = b.x * b.x + b.y * b.y;
            if (sqrMagnitude < 1e-8f)
                return Zero;

            float dot = a.x * b.x + a.y * b.y;
            float scale = dot / sqrMagnitude;
            return new Vector2Int(b.x * scale, b.y * scale);
        }

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.x + b.x, a.y + b.y);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.x - b.x, a.y - b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator -(Vector2Int a) => new(-a.x, -a.y);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new(a.x * b.x, a.y * b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator *(Vector2Int a, float scalar) => new(a.x * scalar, a.y * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator *(float scalar, Vector2Int a) => new(a.x * scalar, a.y * scalar);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator /(Vector2Int a, Vector2Int b) => new(a.x / b.x, a.y / b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator /(Vector2Int a, float scalar) => new(a.x / scalar, a.y / scalar);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2Int a, Vector2Int b) => a.x == b.x && a.y == b.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2Int a, Vector2Int b) => a.x != b.x || a.y != b.y;

        #region Implicit / Explicit

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator (int x, int y)(Vector2Int v) => (v.x, v.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2Int((int x, int y) t) => new Vector2Int(t.x, t.y);

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
            if (!(other is Vector2Int))
                return false;

            return Equals((Vector2Int)other);
        }

        public readonly bool Equals(Vector2Int other) => x == other.x && y == other.y;

        public readonly override int GetHashCode() => HashCode.Combine(x, y);

        #endregion
    }
}
