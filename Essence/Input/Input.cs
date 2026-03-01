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

        }
    }
}
