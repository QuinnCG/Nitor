using Silk.NET.Input;
using SilkKey = Silk.NET.Input.Key;

namespace Nitor.InputManagement;

internal class InputSystem : ILateUpdateSystem
{
	internal static IInputContext Input { get; private set; } = null!;
	internal static IEnumerable<IInputDevice> ConnectedDevices
	{
		get
		{
			return [.. Input.Keyboards, .. Input.Mice, .. Input.Gamepads];
		}
	}

	internal static event Action<SilkKey>? OnKeyDown, OnKeyUp;

	private static readonly HashSet<SilkKey> _keysPressedThisFrame = [], _keysHeld = [], _keysReleasedThisFrame = [];

	public void OnCreate()
	{
		Input = Application.Window.CreateInput();
		Input.ConnectionChanged += OnConnectionChanged;

		foreach (var device in ConnectedDevices)
		{
			Subscribe(device);
		}
	}

	private void OnConnectionChanged(IInputDevice device, bool isConnected)
	{
		if (isConnected)
		{
			Subscribe(device);
		}
		else
		{
			Unsubscribe(device);
		}
	}

	private void Subscribe(IInputDevice device)
	{
		if (device is IKeyboard keyboard)
		{
			keyboard.KeyDown += KeyDown;
			keyboard.KeyUp += KeyUp;
		}
	}

	private void Unsubscribe(IInputDevice device)
	{
		if (device is IKeyboard keyboard)
		{
			keyboard.KeyDown -= KeyDown;
			keyboard.KeyUp -= KeyUp;
		}
	}

	public void OnLateUpdate()
	{
		_keysPressedThisFrame.Clear();
		_keysReleasedThisFrame.Clear();
	}

	public void OnDestroy()
	{
		foreach (var device in ConnectedDevices)
		{
			Unsubscribe(device);
		}
	}

	private void KeyDown(IKeyboard keyboard, SilkKey key, int scancode)
	{
		_keysPressedThisFrame.Add(key);
		_keysHeld.Add(key);

		OnKeyDown?.Invoke(key);
	}

	private void KeyUp(IKeyboard keyboard, SilkKey key, int scancode)
	{
		_keysHeld.Remove(key);
		_keysReleasedThisFrame.Add(key);

		OnKeyUp?.Invoke(key);
	}

	internal static bool IsPressed(SilkKey key) => _keysPressedThisFrame.Contains(key);
	internal static bool IsHeld(SilkKey key) => _keysHeld.Contains(key);
	internal static bool IsReleased(SilkKey key) => _keysReleasedThisFrame.Contains(key);
}
