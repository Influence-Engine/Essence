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

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float value)
        {
            this.x = value;
            this.y = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float x, float y)
        {
            this.x = x; 
            this.y = y;
        }

        #endregion

        #region Common Static Vectors

        public static readonly Vector2 Zero = new Vector2(0f, 0f);
        public static readonly Vector2 One = new Vector2(1f, 1f);

        public static readonly Vector2 Up = new Vector2(0f, 1f);
        public static readonly Vector2 Down = new Vector2(0f, -1f);
        public static readonly Vector2 Left = new Vector2(-1f, 0f);
        public static readonly Vector2 Right = new Vector2(1f, 0f);

        public static readonly Vector2 PositiveInfinity = new(float.PositiveInfinity, float.PositiveInfinity);
        public static readonly Vector2 NegativeInfinity = new(float.NegativeInfinity, float.NegativeInfinity);

        public static readonly Vector2 NegativeOne = new Vector2(-1f, -1f);

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
                return mag > 1e-6f ? new Vector2(x / mag, y / mag) : Zero;
            }
        }

        public readonly Vector2 Perpendicular
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Vector2(-y, x);
        }

        public readonly bool IsZero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => x == 0f && y == 0f;
        }

        public readonly bool IsNaN
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => float.IsNaN(x) || float.IsNaN(y);
        }

        public readonly bool IsFinite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => float.IsFinite(x) && float.IsFinite(y);
        }

        #endregion

        #region Instance Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(float newX, float newY)
        {
            x = newX;
            y = newY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            float magnitude = MathF.Sqrt(x * x + y * y);
            if (magnitude > 1e-6f)
            {
                float inverted = 1f / magnitude;
                x *= inverted;
                y *= inverted;
            }
            else
            {
                x = 0f;
                y = 0f;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClampMagnitude(float maxLength)
        {
            float sqrMagnitude = x * x + y * y;
            if(sqrMagnitude > maxLength * maxLength)
            {
                float scale = maxLength / MathF.Sqrt(sqrMagnitude);
                x *= scale;
                y *= scale;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out float outX, out float outY)
        {
            outX = x;
            outY = y;
        }

        #endregion

        #region Static Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Min(Vector2 a, Vector2 b) => new Vector2(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Max(Vector2 a, Vector2 b) => new Vector2(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Abs(Vector2 a) => new Vector2(MathF.Abs(a.x), MathF.Abs(a.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Clamp(Vector2 vector, Vector2 min, Vector2 max) => new Vector2(Math.Clamp(vector.x, min.x, max.x), Math.Clamp(vector.y, min.y, max.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            float sqrMagnitude = vector.SqrMagnitude;
            if (sqrMagnitude > maxLength * maxLength)
            {
                float scale = maxLength / MathF.Sqrt(sqrMagnitude);
                return new Vector2(vector.x * scale, vector.y * scale);
            }

            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Scale(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector2 a, Vector2 b) => a.x * b.x + a.y * b.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2 a, Vector2 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(Vector2 a, Vector2 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return dx * dx + dy * dy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = Math.Clamp(t, 0f, 1f);
            return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t) => new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SmoothStep(Vector2 a, Vector2 b, float t)
        {
            t = Math.Clamp(t, 0f, 1f);
            t = t * t * (3f - 2f * t);
            return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDelta)
        {
            float dx = target.x - current.x;
            float dy = target.y - current.y;
            float sqrDist = dx * dx + dy * dy;

            if (sqrDist == 0f || (maxDelta >= 0f && sqrDist <= maxDelta * maxDelta))
                return target;

            float dist = MathF.Sqrt(sqrDist);
            return new Vector2(current.x + dx / dist * maxDelta, current.y + dy / dist * maxDelta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Reflect(Vector2 direction, Vector2 normal)
        {
            float dot2 = 2f * (direction.x * normal.x + direction.y * normal.y);
            return new Vector2(direction.x - dot2 * normal.x, direction.y - dot2 * normal.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Project(Vector2 a, Vector2 b)
        {
            float sqrMagnitude = b.x * b.x + b.y * b.y;
            if (sqrMagnitude < 1e-8f)
                return Zero;

            float dot = a.x * b.x + a.y * b.y;
            float scale = dot / sqrMagnitude;
            return new Vector2(b.x * scale, b.y * scale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(Vector2 from, Vector2 to)
        {
            float denominator = MathF.Sqrt(from.SqrMagnitude * to.SqrMagnitude);
            if (denominator < 1e-6f)
                return 0f;

            float dot = Math.Clamp(Dot(from, to) / denominator, -1f, 1f);
            return MathF.Acos(dot) * (180f / MathF.PI);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignedAngle(Vector2 from, Vector2 to)
        {
            float unsigned = Angle(from, to);
            float sign = MathF.Sign(from.x * to.y - from.y * to.x);
            return unsigned * (sign == 0 ? 1f : sign);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 FromAngle(float degrees)
        {
            float rad = degrees * (MathF.PI / 180f);
            return new Vector2(MathF.Cos(rad), MathF.Sin(rad));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximatelyEqual(Vector2 a, Vector2 b, float epsilon = 1e-6f)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return dx * dx + dy * dy <= epsilon * epsilon;
        }

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.x + b.x, a.y + b.y);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.x - b.x, a.y - b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 a) => new(-a.x, -a.y);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 a, float scalar) => new(a.x * scalar, a.y * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(float scalar, Vector2 a) => new(a.x * scalar, a.y * scalar);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.x / b.x, a.y / b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 a, float scalar) => new(a.x / scalar, a.y / scalar);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2 a, Vector2 b) => a.x == b.x && a.y == b.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2 a, Vector2 b) => a.x != b.x || a.y != b.y;

        #region Implicit / Explicit

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator (float x, float y)(Vector2 v) => (v.x, v.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2((float x, float y) t) => new Vector2(t.x, t.y);

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
