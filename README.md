# Essence
A small C# framework for building lightweight applications.  
The pieces you always end up writing yourself.

## Features
- Window: Create and manage an SDL3 window.
- Input: Keyboard and Mouse states with Held/Pressed/Released
- Time: Frame timing with delta time, time scale, fixed timestep, and more.
- Common Structs: Vector2, Color... (more soon)

## Getting Started
Essence is distribtued as a `.dll` library.  
Essence is built on [SDL3](https://github.com/libsdl-org/SDL) and uses a custom [SDL3-CS](https://github.com/Influence-Engine/SDL3-CS) as the C# wrapper. Both are required for Essence to function.  
Every build of Essence includes the current `SDL3.dll` and `SDL3-CS.dll` in `Essence/Managed/Assemblies/`.  

Just add all three to your project references:
- `Essence.dll`
- `SDL3.dll` (native)
- `SDL3-CS.dll` (managed wrapper)

>[!NOTE]
> SDL3 events need to be pumped each frame and routed to ``Window.ProcessEvent`` and ``Input.ProcessEvent``.  
> Essence does not manage the event loop itself.
