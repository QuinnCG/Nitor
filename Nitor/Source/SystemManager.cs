namespace Nitor;

public static class SystemManager
{
	private static readonly HashSet<ISystem> _allSystems = [];
	private static readonly HashSet<IUpdateSystem> _updateSystems = [];
	private static readonly HashSet<ILateUpdateSystem> _lateUpdateSystems = [];
	private static readonly HashSet<ISimulateSystem> _simulateSystems = [];
	private static readonly HashSet<IRenderSystem> _renderSystems = [];

	public static void Register<T>() where T : ISystem, new()
	{
		Register(new T());
	}

	public static void Register(ISystem system)
	{
		if (_allSystems.Add(system))
		{
			if (system is IUpdateSystem update)
			{
				_updateSystems.Add(update);
			}

			if (system is ILateUpdateSystem late)
			{
				_lateUpdateSystems.Add(late);
			}

			if (system is ISimulateSystem simulate)
			{
				_simulateSystems.Add(simulate);
			}

			if (system is IRenderSystem render)
			{
				_renderSystems.Add(render);
			}
		}
	}

	public static void Unregister(ISystem system)
	{
		if (_allSystems.Remove(system))
		{
			if (system is IUpdateSystem update)
			{
				_updateSystems.Remove(update);
			}

			if (system is ILateUpdateSystem late)
			{
				_lateUpdateSystems.Remove(late);
			}

			if (system is ISimulateSystem simulate)
			{
				_simulateSystems.Remove(simulate);
			}

			if (system is IRenderSystem render)
			{
				_renderSystems.Remove(render);
			}
		}
	}

	internal static void Create()
	{
		foreach (var system in _allSystems)
		{
			system.OnCreate();
		}
	}

	internal static void Destroy()
	{
		foreach (var system in _allSystems)
		{
			system.OnDestroy();
		}
	}

	internal static void Update()
	{
		foreach (var system in _updateSystems)
		{
			system.OnUpdate();
		}
	}

	internal static void LateUpdate()
	{
		foreach (var system in _lateUpdateSystems)
		{
			system.OnLateUpdate();
		}
	}

	internal static void Simulate()
	{
		foreach (var system in _simulateSystems)
		{
			system.OnSimulate();
		}
	}

	internal static void Render()
	{
		foreach (var system in _renderSystems)
		{
			system.OnRender();
		}
	}
}
