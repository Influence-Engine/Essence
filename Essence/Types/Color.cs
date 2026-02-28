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

        public float this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= 4)
                    throw new IndexOutOfRangeException();

                return Unsafe.Add(ref r, index);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if ((uint)index >= 4)
                    throw new IndexOutOfRangeException();

                Unsafe.Add(ref r, index) = value;
            }
        }

        #region Constructors

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

        #endregion

        #region Common Static Vectors

        public static readonly Color Red = new Color(1f, 0f, 0f);
        public static readonly Color Green = new Color(0f, 1f, 0f);
        public static readonly Color Blue = new Color(0f, 0f, 1f);

        public static readonly Color Black = new Color(0f, 0f, 0f);
        public static readonly Color White = new Color(1f, 1f, 1f);
        public static readonly Color Gray = new Color(0.5f, 0.5f, 0.5f);

        public static readonly Color Yellow = new Color(1f, 1f, 0f);
        public static readonly Color Cyan = new Color(0f, 1f, 1f);
        public static readonly Color Magenta = new Color(1f, 0f, 1f);

        public static readonly Color Clear = new Color(0f, 0f, 0f, 0f);

        #endregion

        #region Properties

        public readonly Color Clamped
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Math.Clamp(r, 0f, 1f), Math.Clamp(g, 0f, 1f), Math.Clamp(b, 0f, 1f), Math.Clamp(a, 0f, 1f));
        }

        public readonly Color Opaque
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(r, g, b, 1f);
        }

        public readonly float Luminance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 0.2126f * r + 0.7152f * g + 0.0722f * b;
        }

        public readonly Color Grayscale
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                float luminance = 0.2126f * r + 0.7152f * g + 0.0722f * b;
                return new Color(luminance, luminance, luminance, a);
            }
        }

        public readonly Color Inverted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Color(1f - r, 1f - g, 1f - b, a);
        }

        public readonly bool IsOpaque
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => a >= 1f;
        }

        public readonly bool IsTransparent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => a <= 0f;
        }

        #endregion

        #region Instance Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(float newR, float newG, float newB, float newA = 1f)
        {
            r = newR;
            g = newG;
            b = newB;
            a = newA;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp()
        {
            r = Math.Clamp(r, 0f, 1f);
            g = Math.Clamp(g, 0f, 1f);
            b = Math.Clamp(b, 0f, 1f);
            a = Math.Clamp(a, 0f, 1f);
        }

        public readonly Color WithRed(float red) => new(red, g, b, a);

        public readonly Color WithGreen(float green) => new(r, green, b, a);

        public readonly Color WithBlue(float blue) => new(r, g, blue, a);

        public readonly Color WithAlpha(float alpha) => new (r, g, b, alpha);

        #endregion

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
