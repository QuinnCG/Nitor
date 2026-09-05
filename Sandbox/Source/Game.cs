using Nitor;
using Nitor.AssetManagement;
using Nitor.InputManagement;
using Nitor.Math;
using Nitor.Rendering;

namespace Sandbox;

// TODO:
//		- Implement batches.
//		- Implement automatic sprite render system.
//		- Test render performance.
//		- Work on UI.
//			- Render layer should have a special set of UI layers that are part of the index calculation, but always kept with the highest index for UI.
//			- Text rendering.
//			- Support 9 slices.
//		- Work on editor-mode.
//		- Consider custom sprite atlases being made with build pipeline? Would need a system for the asset load to redirect loading.

// TODO: Force require bindless textures always.

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

		var texture = Asset.Load<Texture>("Images/Logo.png");
		var shader = Asset.Load<Shader>("MyShader.shader");

		var myEnt = new Entity(new Transform(), new SpriteRenderer()
		{
			Shader = shader,
			Sprite = new Sprite(texture, new(0f, 0f, 1f, 1f)),
			Color = new Color(1f, 0.5f, 0.5f),
			Pivot = new(0f, -1f)
		});

		var point = new Entity(new Transform(), new SpriteRenderer()
		{
			Shader = shader,
			Sprite = new(texture, new(0f, 0f, 1f, 1f)),
			Scale = Vec2.One * 0.1f,
			Color = Color.Blue
		});

		var cam = new Entity(new Transform(), new CameraView());

		World.Add(myEnt, cam, point);
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
