using Nitor;

namespace Sandbox;

class Game : Application
{
	static void Main()
	{
		var game = new Game();
		game.Run();
	}

	protected override void OnLoad()
	{
		SystemManager.Register<MySystem>();
	}

	protected override void OnClose()
	{
		
	}
}

class MySystem : IUpdateSystem
{
	public void OnUpdate()
	{
		if (Keyboard.Space.IsPressed)
		{
			Console.WriteLine("Spacebar");
		}
	}
}
