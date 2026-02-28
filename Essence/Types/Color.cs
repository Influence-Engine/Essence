using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Essence
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Color : IEquatable<Color>, IFormattable
    {
        public float r;
        public float g;
        public float b;
        public float a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(float r, float g, float b, float a = 1f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(float gray, float a = 1f)
        {
            this.r = gray;
            this.g = gray;
            this.b = gray;
            this.a = a;
        }

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator +(Color a, Color b) => new(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator -(Color a, Color b) => new(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator -(Color a) => new(-a.r, -a.g, -a.b, -a.a);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color a, Color b) => new(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color a, float scalar) => new(a.r * scalar, a.g * scalar, a.b * scalar, a.a * scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(float scalar, Color a) => new(a.r * scalar, a.g * scalar, a.b * scalar, a.a * scalar);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color a, Color b) => new(a.r / b.r, a.g / b.g, a.b / b.b, a.a / b.a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color a, float scalar) => new(a.r / scalar, a.g / scalar, a.b / scalar, a.a / scalar);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Color a, Color b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Color a, Color b) => a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;

        #region Implicit / Explicit

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator (float r, float g, float b, float a)(Color c) => (c.r, c.g, c.b, c.a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color((float r, float g, float b, float a) t) => new Color(t.r, t.g, t.b, t.a);

        #endregion

        #endregion

        #region Common Overrides (ToString, GetHashCode, Equals)

        public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);

        public readonly string ToString(string? format, IFormatProvider? formatProvider = null)
        {
            string fmt = format ?? "G";
            IFormatProvider provider = formatProvider ?? CultureInfo.CurrentCulture;
            return $"({r.ToString(fmt, provider)}, {g.ToString(fmt, provider)}, {b.ToString(fmt, provider)}, {a.ToString(fmt, provider)})";
        }

        public readonly override bool Equals(object? other)
        {
            if (!(other is Color))
                return false;

            return Equals((Color)other);
        }

        public readonly bool Equals(Color other) => r == other.r && g == other.g && b == other.b && a == other.a;

        public readonly override int GetHashCode() => HashCode.Combine(r, g, b, a);

        #endregion
    }
}
