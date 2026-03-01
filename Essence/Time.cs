using System.Runtime.CompilerServices;

namespace Essence
{
    /// <summary>A static time management class. Call <see cref="Time.Update"/> once per frame to advance all time values.</summary>
    public static class Time
    {
        #region Variables

        #region Time

        public static double timeAsDouble { get; private set; }
        public static float time
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)timeAsDouble;
        }

        public static double unscaledTimeAsDouble { get; private set; }
        public static float unscaledTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)unscaledTimeAsDouble;
        }

        public static double deltaTimeAsDouble { get; private set; }
        public static float deltaTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)deltaTimeAsDouble;
        }

        public static double unscaledDeltaTimeAsDouble { get; private set; }
        public static float unscaledDeltaTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)unscaledDeltaTimeAsDouble;
        }

        public static double fixedTimeAsDouble { get; private set; }
        public static float fixedTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)fixedTimeAsDouble;
        }

        public static double fixedDeltaTimeAsDouble { get; private set; } = 1.0 / 50.0;
        public static float fixedDeltaTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)fixedDeltaTimeAsDouble;
        }

        public static double smoothDeltaTimeAsDouble
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => smoothSampleCount > 0 ? smoothDeltaAccumulator / smoothSampleCount : 0.0;
        }

        #endregion

        public static int frameCount { get; private set; }

        public static int fixedFrameCount { get; private set; }

        public static float timeScale = 1f;
        public static float maxDeltaTime = 1f / 3f; // No less than ~3 FPS

        #region Smooth Time

        const int SmoothSamples = 5;

        public static double smoothDeltaAccumulator {  get; private set; }
        public static int smoothSampleCount { get; private set; }
        public static readonly double[] smoothBuffer = new double[SmoothSamples];
        public static int smoothBufferIndex { get; private set; }

        #endregion

        public static double fixedAccumulator { get; private set; }

        public static float FixedRate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)(1.0 / fixedDeltaTimeAsDouble);
        }

        public static float Fps
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => unscaledDeltaTimeAsDouble > 0.0 ? (float)(1.0 / unscaledDeltaTimeAsDouble) : 0f;
        }

        public static float SmoothFps
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => smoothDeltaTimeAsDouble > 0.0 ? (float)(1.0 / smoothDeltaTimeAsDouble) : 0f;
        }

        public static float FixedAlpha
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)(fixedAccumulator / fixedDeltaTimeAsDouble);
        }

        public static bool FixedUpdatedThisFrame
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => fixedAccumulator < fixedDeltaTimeAsDouble;
        }

        #endregion

        /// <summary>
        /// Advances all time values by the given raw frame duration. <br></br>
        /// Call this exactly once per frame before any game logic.
        /// </summary>
        /// <param name="deltaTime">Raw elapsed time in seconds since the last frame.</param>
        public static void Update(double deltaTime)
        {
            double rawDelta = Math.Min(deltaTime, maxDeltaTime);

            unscaledDeltaTimeAsDouble = rawDelta;
            unscaledTimeAsDouble += rawDelta;

            double scaledDelta = rawDelta * timeScale;
            deltaTimeAsDouble = scaledDelta;
            timeAsDouble += scaledDelta;

            smoothDeltaAccumulator -= smoothBuffer[smoothBufferIndex];
            smoothBuffer[smoothBufferIndex] = rawDelta;
            smoothDeltaAccumulator += rawDelta;
            smoothBufferIndex = (smoothBufferIndex + 1) % SmoothSamples;
            if (smoothSampleCount < SmoothSamples)
                smoothSampleCount++;

            fixedAccumulator += scaledDelta;
            while(fixedAccumulator >= fixedDeltaTimeAsDouble)
            {
                fixedTimeAsDouble += fixedDeltaTimeAsDouble;
                fixedAccumulator -= fixedDeltaTimeAsDouble;
                fixedFrameCount++;
            }

            frameCount++;
        }

        /// <summary>
        /// Advances all time values by the given raw frame duration. <br></br>
        /// Call this exactly once per frame before any game logic.
        /// </summary>
        /// <param name="deltaTime">Raw elapsed time in seconds since the last frame.</param>
        public static void Update(float deltaTime) => Update((double)deltaTime);

        public static void SetFixedDeltaTime(double seconds)
        {
            if (seconds <= 0.0)
                throw new ArgumentOutOfRangeException(nameof(seconds), "Fixed delta time must be positive.");

            fixedDeltaTimeAsDouble = seconds;
        }

        public static void SetFixedRate(double hz)
        {
            if (hz <= 0.0)
                throw new ArgumentOutOfRangeException(nameof(hz), "Fixed rate must be positive.");

            fixedDeltaTimeAsDouble = 1.0 / hz;
        }

        /// <summary>Reset all time states to zero.</summary>
        public static void Reset()
        {
            timeAsDouble = 0.0;
            unscaledTimeAsDouble = 0.0;

            deltaTimeAsDouble = 0.0;
            unscaledDeltaTimeAsDouble = 0.0;

            fixedTimeAsDouble = 0.0;
            fixedAccumulator = 0.0;

            frameCount = 0;
            fixedFrameCount = 0;
            timeScale = 1f;

            smoothDeltaAccumulator = 0.0;
            smoothSampleCount = 0;
            smoothBufferIndex = 0;
            Array.Clear(smoothBuffer, 0, SmoothSamples);
        }
    }
}
