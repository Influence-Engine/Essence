using SDL3;
using System.Runtime.CompilerServices;

namespace Essence
{
    /// <summary>
    /// An implementation of Window class using SDL3.<br></br>
    /// Feed SDL events via <see cref="Window.ProcessEvent"/>
    /// </summary>
    public class Window : IDisposable
    {
        nint handle;
        bool disposed;

        #region Cached States

        int width;
        int height;

        int x;
        int y;

        string title;

        #endregion

        #region Properties

        public IntPtr Handle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => handle;
        }

        public bool IsOpen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !disposed && handle != IntPtr.Zero;
        }

        public string Title
        {
            get => title;
            set
            {
                if (title == value)
                    return;

                title = value;
                SDL.SetWindowTitle(handle, value);
            }
        }

        // TODO use Vector2Int
        public Vector2 Size
        {
            get => new Vector2(width, height);
            set => Resize((int)value.x, (int)value.y);
        }

        // TODO use Vector2Int
        public Vector2 Position
        {
            get => new Vector2(x, y);
            set => MoveTo((int)value.x, (int)value.y);
        }

        public float AspectRatio
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => height > 0 ? (float)width / height : 1f;
        }

        #endregion

        public Window(string title, int width, int height)
        {
            if (!SDL.Init(SDL.InitFlags.Video))
                throw new InvalidOperationException($"(SDL Init) failed: {SDL.GetError()}");

            this.title = title;
            this.width = width;
            this.height = height;

            SDL.WindowFlags flags = 0;

            // Create window
            handle = SDL.CreateWindow(title, width, height, flags);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException($"(SDL CreateWindow) failed: {SDL.GetError()}");

            // Cache initial position
            SDL.GetWindowPosition(handle, out x, out y);
        }

        public void ProcessEvent(SDL.Event e)
        {
            switch(e.type)
            {
                // Do stuff here
                case SDL.EventType.WindowResized:
                    HandleResize(e.window.data1, e.window.data2);
                    break;

                case SDL.EventType.WindowMoved:
                    HandleMove(e.window.data1, e.window.data2);
                    break;

                case SDL.EventType.Quit:
                    Close();
                    break;
            }
        }

        #region Functionality

        public void Close()
        {
            if (!IsOpen)
                return;

            // Trigger OnClose Action
            Dispose();
        }

        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException();

            SDL.SetWindowSize(handle, width, height);
        }

        public void MoveTo(int x, int y) => SDL.SetWindowPosition(handle, x, y);

        #endregion

        #region Handlers

        void HandleResize(int newWidth, int newHeight)
        {
            if (newWidth == width && newHeight == height)
                return; // No changes

            width = newWidth;
            height = newHeight;
            // Trigger OnResize Action
        }

        void HandleMove(int newX, int newY)
        {
            if (newX == x && newY == y)
                return; // No changes

            x = newX;
            y = newY;
            // Trigger OnMoved Action
        }

        #endregion

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            if(handle != IntPtr.Zero)
            {
                SDL.DestroyWindow(handle);
                handle = IntPtr.Zero;
            }

            SDL.Quit();
        }
    }
}
