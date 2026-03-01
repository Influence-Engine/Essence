using SDL3;

namespace Essence.Input
{
    /// <summary>
    /// A static input management class. <br></br>
    /// Call <see cref="Input.Update"/> at the start of each frame, then feed SDL events via <see cref="Input.ProcessEvent"/>
    /// </summary>
    public static class Input
    {
        const int KeyCapacity = (int)KeyCode._Count;
        const int MouseButtonCount = 5;

        #region Keyboard State

        static readonly bool[] keyHeld = new bool[KeyCapacity];
        static readonly bool[] keyPressedThisFrame = new bool[KeyCapacity];
        static readonly bool[] keyReleasedThisFrame = new bool[KeyCapacity];

        static readonly List<int> dirtyKeys = new List<int>(32);
        static int anyKeyDownCount;

        #endregion

        #region Mouse Button State

        static readonly bool[] mouseHeld = new bool[MouseButtonCount];
        static readonly bool[] mousePressedThisFrame = new bool[MouseButtonCount];
        static readonly bool[] mouseReleasedThisFrame = new bool[MouseButtonCount];

        static readonly List<int> dirtyMouseButtons = new List<int>(8);
        static int anyMouseDownCoun;

        #endregion

        #region Mouse Position & Movement

        public static Vector2 mousePosition { get; private set; }
        public static Vector2 lastMousePositiob { get; private set; }
        public static Vector2 mouseDelta { get; private set; }
        public static Vector2 mouseScrollDelta { get; private set; }

        #endregion

        static readonly Dictionary<SDL.KeyCode, KeyCode> sdlKeyMap;

        static Input()
        {
            // TODO Complete the list (I am to lazy atm)
            sdlKeyMap = new Dictionary<SDL.KeyCode, KeyCode>(KeyCapacity)
            {
                { SDL.KeyCode.Space, KeyCode.Space },
                { SDL.KeyCode.Space, KeyCode.Apostrophe },
                { SDL.KeyCode.Space, KeyCode.Comma },
                { SDL.KeyCode.Space, KeyCode.Minus },
                { SDL.KeyCode.Space, KeyCode.Period },
                { SDL.KeyCode.Space, KeyCode.Slash },

                { SDL.KeyCode.Alpha0, KeyCode.Alpha0 },
                { SDL.KeyCode.Alpha1, KeyCode.Alpha1 },
                { SDL.KeyCode.Alpha2, KeyCode.Alpha2 },
                { SDL.KeyCode.Alpha3, KeyCode.Alpha3 },
                { SDL.KeyCode.Alpha4, KeyCode.Alpha4 },
                { SDL.KeyCode.Alpha5, KeyCode.Alpha5 },
                { SDL.KeyCode.Alpha6, KeyCode.Alpha6 },
                { SDL.KeyCode.Alpha7, KeyCode.Alpha7 },
                { SDL.KeyCode.Alpha8, KeyCode.Alpha8 },
                { SDL.KeyCode.Alpha9, KeyCode.Alpha9 },

                { SDL.KeyCode.SemiColon, KeyCode.Semicolon },
                { SDL.KeyCode.Equals, KeyCode.Equal },

                { SDL.KeyCode.A, KeyCode.A },
                { SDL.KeyCode.B, KeyCode.B },
                { SDL.KeyCode.C, KeyCode.C },
                { SDL.KeyCode.D, KeyCode.D },
                { SDL.KeyCode.E, KeyCode.E },
                { SDL.KeyCode.F, KeyCode.F },
                { SDL.KeyCode.G, KeyCode.G },
                { SDL.KeyCode.H, KeyCode.H },
                { SDL.KeyCode.I, KeyCode.I },
                { SDL.KeyCode.J, KeyCode.J },
                { SDL.KeyCode.K, KeyCode.K },
                { SDL.KeyCode.L, KeyCode.L },
                { SDL.KeyCode.M, KeyCode.M },
                { SDL.KeyCode.N, KeyCode.N },
                { SDL.KeyCode.O, KeyCode.O },
                { SDL.KeyCode.P, KeyCode.P },
                { SDL.KeyCode.Q, KeyCode.Q },
                { SDL.KeyCode.R, KeyCode.R },
                { SDL.KeyCode.S, KeyCode.S },
                { SDL.KeyCode.T, KeyCode.T },
                { SDL.KeyCode.U, KeyCode.U },
                { SDL.KeyCode.V, KeyCode.V },
                { SDL.KeyCode.W, KeyCode.W },
                { SDL.KeyCode.X, KeyCode.X },
                { SDL.KeyCode.Y, KeyCode.Y },
                { SDL.KeyCode.Z, KeyCode.Z },

                { SDL.KeyCode.LeftBracket, KeyCode.LeftBracket },
                { SDL.KeyCode.Backslash, KeyCode.BackSlash },
                { SDL.KeyCode.RightBracket, KeyCode.RightBracket },
                // GraveAccent

                { SDL.KeyCode.Escape, KeyCode.RightBracket },
                { SDL.KeyCode.RightBracket, KeyCode.RightBracket },
                { SDL.KeyCode.Tab, KeyCode.RightBracket },
                { SDL.KeyCode.Backspace, KeyCode.Backspace },
                { SDL.KeyCode.Insert, KeyCode.Insert },
                { SDL.KeyCode.Delete, KeyCode.Delete },

                { SDL.KeyCode.Up, KeyCode.Up },
                { SDL.KeyCode.Down, KeyCode.Down },
                { SDL.KeyCode.Left, KeyCode.Left },
                { SDL.KeyCode.Right, KeyCode.Right },

                { SDL.KeyCode.LeftShift, KeyCode.LeftShift },
                { SDL.KeyCode.LeftCtrl, KeyCode.LeftControl },
                { SDL.KeyCode.LeftAlt, KeyCode.LeftAlt },
                // Left Super
                { SDL.KeyCode.RightShift, KeyCode.RightShift },
                { SDL.KeyCode.RightCtrl, KeyCode.RightControl },
                { SDL.KeyCode.RightAlt, KeyCode.RightAlt },
                // Right Super
                { SDL.KeyCode.Z, KeyCode.Z },
            };
        }
    }
}
