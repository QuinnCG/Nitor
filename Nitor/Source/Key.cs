using Silk.NET.Input;
using SilkKey = Silk.NET.Input.Key;

namespace Nitor;

public class Key : IBinaryInput
{
	public bool IsPressed => InputSystem.IsPressed(_key);
	public bool IsHeld => InputSystem.IsHeld(_key);
	public bool IsReleased => InputSystem.IsReleased(_key);

	private readonly SilkKey _key;

	internal Key(SilkKey key)
	{
		_key = key;
	}
}
