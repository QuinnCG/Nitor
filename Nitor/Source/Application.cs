using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;

namespace Nitor;

public abstract class Application
{
	/// <summary>
	/// Is this build a debug build?
	/// </summary>
	public static bool IsDebug
	{
		get
		{
#if DEBUG
			return true;
#else
			return false;
#endif
		}
	}
	/// <summary>
	/// Is this build a release build?
	/// </summary>
	public static bool IsRelease => !IsDebug;

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
		SystemManager.Register<World>();
		SystemManager.Register<AssetManagement.AssetAddressSystem>();
		SystemManager.Register<InputManagement.InputSystem>();
		SystemManager.Register<Rendering.Renderer>();
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
