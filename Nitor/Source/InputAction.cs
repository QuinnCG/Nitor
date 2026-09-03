namespace Nitor;

public class InputAction(params IEnumerable<IBinaryInput> inputs) : IBinaryInput
{
	internal IEnumerable<IBinaryInput> Inputs { get; } = inputs;

	public bool IsPressed => Inputs.Any(x => x.IsPressed);
	public bool IsHeld => Inputs.Any(x => x.IsHeld);
	public bool IsReleased => Inputs.Any(x => x.IsReleased);
}
