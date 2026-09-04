using Nitor.Math;
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
	public static bool IsClosing => Window.IsClosing;
	public static Vec2i WindowSize
	{
		get
		{
			var size = Window.Size;
			return new(size.X, size.Y);
		}
	}
	public static float WindowRatio => (float)WindowSize.X / WindowSize.Y;

	internal static IWindow Window { get; private set; } = null!;

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
		SystemManager.Register<InputManagement.InputSystem>();
		SystemManager.Register<Rendering.Renderer>();
		SystemManager.Register<AssetManagement.AssetAddressSystem>();
		SystemManager.Register<Rendering.SpriteRenderSystem>();
	}

	private void Update(double dt)
	{
		SystemManager.Update();
		SystemManager.LateUpdate();

		// TODO: SystemManager.Simulate();

		Time.Step((float)dt);
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
