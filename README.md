# Nitor

Nitor is a C# 2D game engine prototype targeting .NET 10. It uses Silk.NET for windowing, input, and OpenGL. The included sandbox shows how an application uses the engine to load assets, create entities, and render sprites.

## Run the sandbox

With the .NET 10 SDK installed, run this command from the repository root:

```sh
dotnet run --project Sandbox/Sandbox.csproj
```

The sandbox project includes a build target that invokes the asset packer on [Sandbox/Assets](Sandbox/Assets). In the sandbox, hold **Space** to spawn sprites with randomized positions and colors, hold **Backspace** to remove entities, or press **Escape** to quit.

## How it fits together

- **[Engine](Nitor/Source)** — Entities are assembled from components such as transforms, sprite renderers, and cameras. The [system manager](Nitor/Source/SystemManager.cs) registers systems for update, late update, simulation, and rendering according to the interfaces they implement.
- **[Asset packer](AssetPacker/Source/Packer.cs)** — A separate build tool selects converters by file extension, converts assets asynchronously, and creates a manifest alongside the packed data.
- **[Sandbox](Sandbox/Source/Game.cs)** — The example application loads a texture and shaders, creates a camera, and uses a custom update system to handle keyboard input. Start here to see how the engine’s pieces work together.
