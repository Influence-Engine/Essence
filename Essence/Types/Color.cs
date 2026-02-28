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

        #region Static Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Min(Color a, Color b) => new Color(MathF.Min(a.r, b.r), MathF.Min(a.g, b.g), MathF.Min(a.b, b.b), MathF.Min(a.a, b.a));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Max(Color a, Color b) => new Color(MathF.Max(a.r, b.r), MathF.Max(a.g, b.g), MathF.Max(a.b, b.b), MathF.Max(a.a, b.a));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Math.Clamp(t, 0f, 1f);
            return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color LerpUnclamped(Color a, Color b, float t) => new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximatelyEqual(Color a, Color b, float epsilon = 1e-6f)
            => MathF.Abs(a.r - b.r) <= epsilon
            && MathF.Abs(a.g - b.g) <= epsilon
            && MathF.Abs(a.b - b.b) <= epsilon
            && MathF.Abs(a.a - b.a) <= epsilon;

        #endregion

        #region Conversion Methods

        #region Integer Color Space

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint ToRGBA32()
        {
            uint red = (uint)Math.Clamp((int)(r * 255f + 0.5f), 0, 255);
            uint green = (uint)Math.Clamp((int)(g * 255f + 0.5f), 0, 255);
            uint blue = (uint)Math.Clamp((int)(b * 255f + 0.5f), 0, 255);
            uint alpha = (uint)Math.Clamp((int)(a * 255f + 0.5f), 0, 255);
            return (red << 24) | (green << 16) | (blue << 8) | alpha;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromRGBA32(uint rgba)
        {
            const float inverted = 1f / 255f;
            return new Color(
                ((rgba >> 24) & 0xFF) * inverted,
                ((rgba >> 16) & 0xFF) * inverted,
                ((rgba >> 8) & 0xFF) * inverted,
                (rgba & 0xFF) * inverted);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint ToARGB32()
        {
            uint red = (uint)Math.Clamp((int)(r * 255f + 0.5f), 0, 255);
            uint green = (uint)Math.Clamp((int)(g * 255f + 0.5f), 0, 255);
            uint blue = (uint)Math.Clamp((int)(b * 255f + 0.5f), 0, 255);
            uint alpha = (uint)Math.Clamp((int)(a * 255f + 0.5f), 0, 255);
            return (alpha << 24) | (red << 16) | (green << 8) | blue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromARGB32(uint rgba)
        {
            const float inverted = 1f / 255f;
            return new Color(
                ((rgba >> 16) & 0xFF) * inverted,
                ((rgba >> 8) & 0xFF) * inverted,
                (rgba & 0xFF) * inverted,
                ((rgba >> 24) & 0xFF) * inverted);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromBytes(byte r, byte g, byte b, byte a = 255)
        {
            const float inverted = 1f / 255f;
            return new Color(r * inverted, g * inverted, b * inverted, a * inverted);
        }

        #endregion

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
