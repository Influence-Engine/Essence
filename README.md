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

## Usage
### Base App Example
```cs
using Essence;
using Essence.Input;
using SDL3;

// Init Video for Window
if (!SDL.Init(SDL.InitFlags.Video))
  return;

// Create Window
Window window = new Window("Essence App", 1280, 720);

float deltaTime = 0f;
ulong lastTime = SDL.GetTicks();

// Run active loop
while(window.IsOpen)
{
  // Calculate time
  ulong currentTime = SDL.GetTicks();
  deltaTime = (currentTime - lastTime) / 1000f;
  lastTime = currentTime;

  // Update at start of frame
  Time.Update(deltaTime);
  Input.Update();

  // Poll and Process Events
  while(SDL.PollEvent(out SDL.Event e))
  {
    window.ProcessEvent(e);
    Input.ProcessEvent(e);
  }

  // Your update and render logic here
}

window.Dispose();
```

## Roadmap
- [ ] Application base class
- [ ] Camera
- [ ] More structs
- [ ] Complete Input Keycodes
- [ ] UI Support
