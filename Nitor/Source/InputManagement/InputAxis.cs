namespace Nitor.InputManagement;

public class InputAxis : IAnalogueInput
{
	public float Value
	{
		// TODO: Smart handling of priority.

		get
		{
			if (_positive.Any(x => x.IsHeld))
			{
				return 1f;
			}
			else if (_negative.Any(x => x.IsHeld))
			{
				return -1f;
			}
			else
			{
				return 0f;
			}
		}
	}

	private readonly IEnumerable<IBinaryInput> _positive, _negative;

	public InputAxis(InputAction positve, InputAction negative)
	{
		_positive = positve.Inputs;
		_negative = negative.Inputs;
	}
	public InputAxis(Key positive, Key negative)
	{
		_positive = [positive];
		_negative = [negative];
	}
}
