using SDL3;
using System.Runtime.CompilerServices;

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
        static int anyMouseDownCount;

        #endregion

        #region Mouse Position & Movement

        public static Vector2 mousePosition { get; private set; }
        public static Vector2 lastMousePosition { get; private set; }
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
                { SDL.KeyCode.Quote, KeyCode.Apostrophe },
                { SDL.KeyCode.Comma, KeyCode.Comma },
                { SDL.KeyCode.Minus, KeyCode.Minus },
                { SDL.KeyCode.Period, KeyCode.Period },
                { SDL.KeyCode.Slash, KeyCode.Slash },

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

        public static void Update()
        {
            foreach(int i in dirtyKeys)
            {
                keyPressedThisFrame[i] = false;
                keyReleasedThisFrame[i] = false;
            }
            dirtyKeys.Clear();
            anyKeyDownCount = 0;

            foreach(int i in dirtyMouseButtons)
            {
                mousePressedThisFrame[i] = false;
                mouseReleasedThisFrame[i] = false;
            }
            dirtyMouseButtons.Clear();
            anyMouseDownCount = 0;

            mouseDelta = mousePosition - lastMousePosition;
            lastMousePosition = mousePosition;
            mouseScrollDelta = Vector2.Zero;
        }

        public static void ProcessEvent(SDL.Event e)
        {
            switch(e.type)
            {
                case SDL.EventType.KeyDown:
                    if (!e.key.repeat) 
                        OnKeyDown(e.key.key);
                    break;

                case SDL.EventType.KeyUp:
                    OnKeyUp(e.key.key);
                    break;

                case SDL.EventType.MouseButtonDown:
                    OnMouseButtonDown(GetMouseButtonIndex(e.button.button));
                    break;

                case SDL.EventType.MouseButtonUp:
                    OnMouseButtonDown(GetMouseButtonIndex(e.button.button));
                    break;

                case SDL.EventType.MouseMotion:
                    mousePosition = new Vector2(e.motion.x, e.motion.y);
                    break;

                case SDL.EventType.MouseWheel:
                    mouseScrollDelta = new Vector2(e.wheel.x, e.wheel.y);
                    break;
            }
        }

        #region Event Handling

        static void OnKeyDown(SDL.KeyCode sdlKey)
        {
            if (!sdlKeyMap.TryGetValue(sdlKey, out KeyCode key))
                return;

            int i = (int)key;
            if (keyHeld[i])
                return;

            keyHeld[i] = true;
            keyPressedThisFrame[i] = true;
            anyKeyDownCount++;
            dirtyKeys.Add(i);
        }

        static void OnKeyUp(SDL.KeyCode sdlKey)
        {
            if (!sdlKeyMap.TryGetValue(sdlKey, out KeyCode key))
                return;

            int i = (int)key;
            if (!keyHeld[i])
                return;

            keyHeld[i] = false;
            keyReleasedThisFrame[i] = true;
            dirtyKeys.Add(i);
        }

        static void OnMouseButtonDown(int button)
        {
            if ((uint)button >= MouseButtonCount)
                return;

            if (mouseHeld[button])
                return;

            mouseHeld[button] = true;
            mousePressedThisFrame[button] = true;
            anyMouseDownCount++;
            dirtyMouseButtons.Add(button);
        }

        static void OnMouseButtonUp(int button)
        {
            if ((uint)button >= MouseButtonCount)
                return;

            if (!mouseHeld[button])
                return;

            mouseHeld[button] = false;
            mousePressedThisFrame[button] = true;
            dirtyMouseButtons.Add(button);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetMouseButtonIndex(uint sdlButton) => sdlButton switch
        {
            1 => 0, // Left
            2 => 2, // Middle
            3 => 1, // Right
            4 => 3, // X1
            5 => 4, // X2
            _ => -1
        };

        #endregion

        #region Keyboard Queries

        public static bool AnyKeyDown
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => anyKeyDownCount > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetKey(KeyCode key)
        {
            int i = (int)key;
            return (uint)i < KeyCapacity && keyHeld[i];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetKeyDown(KeyCode key)
        {
            int i = (int)key;
            return (uint)i < KeyCapacity && keyPressedThisFrame[i];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetKeyUp(KeyCode key)
        {
            int i = (int)key;
            return (uint)i < KeyCapacity && keyReleasedThisFrame[i];
        }

        #endregion

        #region Mouse Queries

        public static bool AnyMouseButtonDown
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => anyMouseDownCount > 0;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetMouseButton(uint button) => button < MouseButtonCount && mouseHeld[(int)button];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetMouseButton(MouseButton button) => GetMouseButton((uint)button);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetMouseButtonDown(uint button) => button < MouseButtonCount && mousePressedThisFrame[(int)button];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetMouseButtonDown(MouseButton button) => GetMouseButtonDown((uint)button);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetMouseButtonUp(uint button) => button < MouseButtonCount && mouseReleasedThisFrame[(int)button];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetMouseButtonUp(MouseButton button) => GetMouseButton((uint)button);

        #endregion

        public static void Reset()
        {
            Array.Clear(keyHeld, 0, KeyCapacity);
            Array.Clear(keyPressedThisFrame, 0, KeyCapacity);
            Array.Clear(keyReleasedThisFrame, 0, KeyCapacity);
            dirtyKeys.Clear();
            anyKeyDownCount = 0;

            Array.Clear(mouseHeld, 0, MouseButtonCount);
            Array.Clear(mousePressedThisFrame, 0, MouseButtonCount);
            Array.Clear(mouseReleasedThisFrame, 0, MouseButtonCount);
            dirtyMouseButtons.Clear();
            anyMouseDownCount = 0;

            mousePosition = Vector2.Zero;
            lastMousePosition = Vector2.Zero;
            mouseDelta = Vector2.Zero;
        }
    }
}
