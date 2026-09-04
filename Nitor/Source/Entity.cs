using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Nitor;

public class Entity : IEnumerable<Component>
{
	public bool WasCreated { get; private set; }
	public bool WasDestroyed { get; private set; }

	public event Action<Component>? OnComponentAdded, OnComponentRemoved;

	private readonly HashSet<Component> _components = [];

	internal void Create()
	{
		if (!WasCreated && !WasDestroyed)
		{
			WasCreated = true;

			foreach (var component in _components)
			{
				component.Create(this);
				ValidateComponentRequirements(component);
			}

			OnCreate();
		}
	}

	public void Destroy()
	{
		if (WasCreated && !WasDestroyed)
		{
			WasDestroyed = true;

			OnDestroy();

			foreach (var component in _components)
			{
				component.Destroy();
			}

			World.Remove(this);
		}
	}

	public T CreateComponent<T>() where T : Component, new()
	{
		var comp = new T();
		AddComponent(comp);

		return comp;
	}
	public void AddComponent(params IEnumerable<Component> components)
	{
		foreach (var comp in components)
		{
			if (comp.Entity != null)
			{
				throw new Exception("Entity already has a parent. Cannot add another!");
			}

			_components.Add(comp);
			comp.Create(this);

			OnComponentAdded?.Invoke(comp);
		}

		foreach (var comp in components)
		{
			ValidateComponentRequirements(comp);
		}
	}

	[Conditional("DEBUG")]
	private void ValidateComponentRequirements(Component component)
	{
		foreach (var attribute in component.GetType().GetCustomAttributes())
		{
			var type = attribute.GetType();

			if (!type.IsGenericType)
			{
				continue;
			}

			if (type.GetGenericTypeDefinition() == typeof(RequireSiblingAttribute<>))
			{
				var requiredType = type.GetGenericArguments().First();

				if (!HasComponent(requiredType))
				{
					throw new Exception($"Component {component.GetType()} on entity {this} requires sibling component {requiredType.Name}!");
				}
			}
		}
	}

	public bool HasComponent(Component component)
	{
		return _components.Contains(component);
	}
	public bool HasComponent<T>() where T : Component
	{
		var type = typeof(T);

		foreach (var comp in _components)
		{
			if (comp.GetType() == type)
			{
				return true;
			}
		}

		return false;
	}
	public bool HasComponent(Type type)
	{
		foreach (var comp in _components)
		{
			if (comp.GetType() == type)
			{
				return true;
			}
		}

		return false;
	}
	public bool HasComponentDerivedFrom<T>() where T : Component
	{
		var type = typeof(T);

		foreach (var comp in _components)
		{
			if (comp.GetType().IsAssignableTo(type))
			{
				return true;
			}
		}

		return false;
	}
	public bool HasComponentDerivedFrom(Type type)
	{
		foreach (var comp in _components)
		{
			if (comp.GetType().IsAssignableTo(type))
			{
				return true;
			}
		}

		return false;
	}



	public T GetFirstComponent<T>() where T : Component
	{
		var type = typeof(T);

		foreach (var comp in _components)
		{
			if (comp.GetType() == type)
			{
				return (T)comp;
			}
		}

		throw new NullReferenceException($"No component of type {type.Name}!");
	}

	public bool TryGetFirstComponent<T>(out T? result) where T : Component
	{
		var type = typeof(T);

		foreach (var comp in _components)
		{
			if (comp.GetType() == type)
			{
				result = (T)comp;
				return true;
			}
		}

		result = null;
		return false;
	}

	public IEnumerable<T> GetAllComponents<T>() where T : Component
	{
		var type = typeof(T);
		return _components.Where(x => x.GetType() == type).Select(x => (T)x);
	}

	public IEnumerable<T> GetAllComponentsDerived<T>() where T : Component
	{
		var type = typeof(T);
		return _components.Where(x => x.GetType().IsAssignableTo(type)).Select(x => (T)x);
	}

	public void DestroyComponent(Component component)
	{
		if (_components.Contains(component))
		{
			component.Destroy();
			_components.Remove(component);

			OnComponentRemoved?.Invoke(component);
		}
	}

	public IEnumerator<Component> GetEnumerator()
	{
		return ((IEnumerable<Component>)_components).GetEnumerator();
	}
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)_components).GetEnumerator();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnDestroy() { }
}
