using Silk.NET.OpenGL;

namespace Nitor.Rendering;

public class Renderer : IRenderSystem
{
	public static Color ClearColor { get; set; } = Color.Black;
	internal static GL OpenGL { get; private set; } = null!;

	private static readonly List<IRenderable> _renderables = [];

	private static readonly Dictionary<IRenderable, HashSet<Action>> _beforeRendercallbacks = [], _afterRenderCallbacks = [];
	private static readonly Dictionary<Action, IRenderable> _callbackToRenderable = [];

	public void OnCreate()
	{
		OpenGL = GL.GetApi(Application.Window);
	}
	public void OnDestroy()
	{
		OpenGL = null!;
	}

	public static void Register(IRenderable renderable)
	{
		if (!_renderables.Contains(renderable))
		{
			_renderables.Add(renderable);
		}
	}
	public static void Unregister(IRenderable renderable)
	{
		_renderables.Remove(renderable);
	}

	/// <summary>
	/// Register a callback to be called just before the specified render target is rendered.
	/// </summary>
	public static void RegisterCallbackBeforeRender(IRenderable target, Action callback)
	{
		if (_beforeRendercallbacks.TryGetValue(target, out var set))
		{
			set.Add(callback);
		}
		else
		{
			_beforeRendercallbacks.Add(target, [callback]);
		}
	}
	/// <summary>
	/// Register a callback to be called just after the specified render target is rendered.
	/// </summary>
	public static void RegisterCallbackAfterRender(IRenderable target, Action callback)
	{
		if (_afterRenderCallbacks.TryGetValue(target, out var set))
		{
			set.Add(callback);
		}
		else
		{
			_afterRenderCallbacks.Add(target, [callback]);
		}
	}
	/// <summary>
	/// Unregister this callback from any registration of before render or after render.
	/// </summary>
	public static void UnregisterRenderCallback(Action callback)
	{
		if (_callbackToRenderable.TryGetValue(callback, out var renderable))
		{
			if (_beforeRendercallbacks.TryGetValue(renderable, out var set))
			{
				set.Remove(callback);
			}

			if (_afterRenderCallbacks.TryGetValue(renderable, out set))
			{
				set.Remove(callback);
			}
		}
	}

	private int Compare(IRenderable a, IRenderable b)
	{
		if (a.BlendMode != b.BlendMode)
		{
			return (a.BlendMode is BlendMode.Translucent) ? -1 : 1;
		}

		if (a.Layer.Index != b.Layer.Index)
		{
			return (a.Layer.Index < b.Layer.Index) ? -1 : 1;
		}

		if (a.Texture != b.Texture)
		{
			return a.Texture.Path.CompareTo(b.Texture.Path);
		}

		if (a.Shader != b.Shader)
		{
			return a.Shader.Path.CompareTo(b.Shader.Path);
		}

		return 0;
	}
	public void OnRender()
	{
		_renderables.Sort(Compare);

		OpenGL.ClearColor(ClearColor.R, ClearColor.G, ClearColor.B, ClearColor.A);
		OpenGL.Clear(ClearBufferMask.ColorBufferBit);

		foreach (var renderable in _renderables)
		{
			if (_beforeRendercallbacks.TryGetValue(renderable, out var set))
			{
				foreach (var callback in set)
				{
					callback.Invoke();
				}
			}

			renderable.OnRender();

			if (_afterRenderCallbacks.TryGetValue(renderable, out set))
			{
				foreach (var callback in set)
				{
					callback.Invoke();
				}
			}
		}
	}
}
