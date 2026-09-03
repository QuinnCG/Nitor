using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;

namespace Nitor;

public abstract class Application
{
	internal static event Action? OnPreLoad;
	public static event Action? OnLoaded;
	internal static IWindow Window { get; private set; } = null!;

	public static bool IsClosing => Window.IsClosing;

	public void Run()
	{
		Window = SilkWindow.Create(WindowOptions.Default);
		Window.Load += Load;
		Window.Update += Update;
		Window.Render += Render;
		Window.Closing += Close;
		Window.Run();
	}

	private void Load()
	{
		RegisterDefaultSystems();
		SystemManager.Create();

		OnLoad();
	}

	private static void RegisterDefaultSystems()
	{
		SystemManager.Register<AssetAddressSystem>();
		SystemManager.Register<InputSystem>();
		SystemManager.Register<Rendering.RendererSystem>();
	}

	private void Update(double dt)
	{
		SystemManager.Update();
		SystemManager.LateUpdate();

		// TODO: SystemManager.Simulate();
	}

	private void Render(double dt)
	{
		SystemManager.Render();
	}

	public static void Quit()
	{
		Window.Close();
	}

	private void Close()
	{
		OnClose();
		SystemManager.Destroy();
	}

	protected virtual void OnLoad() { }
	protected virtual void OnClose() { }
}
