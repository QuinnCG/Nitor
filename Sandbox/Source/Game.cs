using Nitor;
using Nitor.AssetManagement;
using Nitor.InputManagement;
using Nitor.Rendering;

namespace Sandbox;

// TODO: Implement texture.
// Implement batches.
// Implement automatic sprite render system.
// Test render performance.
// Work on UI. Maybe special render batch that can use depth and bindless textures?
// Work on editor-mode.

class Game : Application
{
	static void Main()
	{
		var game = new Game();
		game.Run();
	}

	protected override async void OnLoad()
	{
		SystemManager.Register<MySystem>();

		var mySprite = new MySprite();
		Renderer.Register(mySprite);

		var ent = new Entity();
		ent.AddComponent(new Transform(), new CameraView());
	}
}

class MySprite : IRenderable
{
	public Shader Shader { get; }
	public Texture Texture { get; }
	public BlendMode BlendMode { get; set; }
	public RenderLayer Layer { get; set; } = RenderLayer.Default;

	private uint _vao;

	public unsafe MySprite()
	{
		Shader = Asset.Load<Shader>("MyShader.shader");
		Texture = Asset.Load<Texture>("Images/Logo.png");

		var gl = Renderer.OpenGL;

		_vao = gl.CreateVertexArray();
		gl.BindVertexArray(_vao);

		var vertices = new float[]
		{
			-0.5f, -0.5f,	0f, 0f,
			-0.5f,  0.5f,	0f, 1f,
			 0.5f,  0.5f,	1f, 1f,
			 0.5f, -0.5f,	1f, 0f
		};

		var indices = new uint[]
		{
			0, 1, 2,
			3, 0, 2
		};

		var vbo = gl.CreateBuffer();
		gl.BindBuffer(Silk.NET.OpenGL.BufferTargetARB.ArrayBuffer, vbo);
		gl.BufferData(Silk.NET.OpenGL.BufferTargetARB.ArrayBuffer, new ReadOnlySpan<float>(vertices), Silk.NET.OpenGL.BufferUsageARB.StaticDraw);

		var ibo = gl.CreateBuffer();
		gl.BindBuffer(Silk.NET.OpenGL.BufferTargetARB.ElementArrayBuffer, ibo);
		gl.BufferData(Silk.NET.OpenGL.BufferTargetARB.ElementArrayBuffer, new ReadOnlySpan<uint>(indices), Silk.NET.OpenGL.BufferUsageARB.StaticDraw);

		uint stride = sizeof(float) * 4;

		gl.EnableVertexAttribArray(0);
		gl.VertexAttribPointer(0, 2, Silk.NET.OpenGL.VertexAttribPointerType.Float, false, stride, 0);

		gl.EnableVertexAttribArray(1);
		gl.VertexAttribPointer(1, 2, Silk.NET.OpenGL.VertexAttribPointerType.Float, false, stride, sizeof(float) * 2);

		gl.Disable(Silk.NET.OpenGL.EnableCap.CullFace);
	}

	public unsafe void OnRender()
	{
		var gl = Renderer.OpenGL;

		gl.BindVertexArray(_vao);
		gl.DrawElements(Silk.NET.OpenGL.PrimitiveType.Triangles, 6, Silk.NET.OpenGL.DrawElementsType.UnsignedInt, null);
	}
}

class MySystem : IUpdateSystem
{
	public void OnUpdate()
	{
		if (Keyboard.Escape.IsPressed)
		{
			Application.Quit();
		}
	}
}
