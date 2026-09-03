namespace Nitor;

public abstract class Component
{
	public Entity Entity { get; private set; } = null!;
	public bool WasCreated { get; private set; }
	public bool WasDestroyed { get; private set; }

	public T GetFirstSibling<T>() where T : Component
	{
		return Entity.GetFirstComponent<T>();
	}

	public bool TryGetFirstSibling<T>(out T? result) where T : Component
	{
		return Entity.TryGetFirstComponent<T>(out result);
	}

	public IEnumerable<T> GetAllSiblings<T>() where T : Component
	{
		return Entity.GetAllComponents<T>();
	}

	public IEnumerable<T> GetAllSiblingsDerived<T>() where T : Component
	{
		return Entity.GetAllComponents<T>();
	}

	internal void Create(Entity entity)
	{
		if (!WasCreated && !WasDestroyed)
		{
			WasCreated = true;
			Entity = entity;
			OnCreate();
		}
		else
		{
			throw new Exception("Cannot create component that was already created!");
		}
	}
	internal void Destroy()
	{
		if (WasCreated && !WasDestroyed)
		{
			WasDestroyed = true;
			OnDestroy();
		}
	}

	protected virtual void OnCreate() { }
	protected virtual void OnDestroy() { }
}
