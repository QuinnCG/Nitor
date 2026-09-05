using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nitor.Rendering;

namespace Nitor;

public class World : IUpdateSystem, ILateUpdateSystem, ISimulateSystem
{
	public static int EntityCount => _entities.Count;
	public static IEnumerable<Entity> Entities => _entities;

	private static readonly HashSet<Entity> _entities = [];
	private static readonly HashSet<Entity> _destroyed = [];

	private static readonly HashSet<IUpdatable> _update = [];
	private static readonly HashSet<ILateUpdatable> _lateUpdate = [];
	private static readonly HashSet<ISimulatable> _simulate = [];

	public static void Create(params IEnumerable<Entity> entities)
	{
		foreach (var entity in entities)
		{
			if (_entities.Add(entity))
			{
				entity.Create();

				if (entity is IUpdatable update)
				{
					_update.Add(update);
				}

				if (entity is ILateUpdatable lateUpdate)
				{
					_lateUpdate.Add(lateUpdate);
				}

				if (entity is ISimulatable simulate)
				{
					_simulate.Add(simulate);
				}

				if (entity is IRenderable render)
				{
					Renderer.Register(render);
				}

				foreach (var comp in entity)
				{
					OnComponentAdded(comp);
				}

				entity.OnComponentAdded += OnComponentAdded;
				entity.OnComponentRemoved += OnComponentRemoved;
			}
		}
	}

	public static void Destroy(Entity entity)
	{
		if (_entities.Contains(entity) && _destroyed.Add(entity))
		{
			entity.OnComponentAdded -= OnComponentAdded;
			entity.OnComponentRemoved -= OnComponentRemoved;

			entity.Destroy();

			if (entity is IUpdatable update)
			{
				_update.Remove(update);
			}

			if (entity is ILateUpdatable lateUpdate)
			{
				_lateUpdate.Remove(lateUpdate);
			}

			if (entity is ISimulatable simulate)
			{
				_simulate.Remove(simulate);
			}

			if (entity is IRenderable render)
			{
				Renderer.Unregister(render);
			}

			foreach (var comp in entity)
			{
				OnComponentRemoved(comp);
			}
		}
	}

	private static void OnComponentAdded(Component component)
	{
		if (component is IUpdatable update)
		{
			_update.Add(update);
		}

		if (component is ILateUpdatable lateUpdate)
		{
			_lateUpdate.Add(lateUpdate);
		}

		if (component is ISimulatable simulate)
		{
			_simulate.Add(simulate);
		}

		if (component is IRenderable render)
		{
			Renderer.Register(render);
		}
	}

	private static void OnComponentRemoved(Component component)
	{
		if (component is IUpdatable update)
		{
			_update.Remove(update);
		}

		if (component is ILateUpdatable lateUpdate)
		{
			_lateUpdate.Remove(lateUpdate);
		}

		if (component is ISimulatable simulate)
		{
			_simulate.Remove(simulate);
		}

		if (component is IRenderable render)
		{
			Renderer.Register(render);
		}
	}

	public void OnUpdate()
	{
		foreach (var update in _update)
		{
			update.OnUpdate();
		}
	}

	public void OnLateUpdate()
	{
		foreach (var late in _lateUpdate)
		{
			late.OnLateUpdate();
		}

		foreach (var entity in _destroyed)
		{
			_entities.Remove(entity);
		}

		_destroyed.Clear();
	}

	public void OnSimulate()
	{
		foreach (var simulate in _simulate)
		{
			simulate.OnSimulate();
		}
	}
}
