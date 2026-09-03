namespace Nitor;

public interface IBinaryInput
{
	public bool IsPressed { get; }
	public bool IsHeld { get; }
	public bool IsReleased { get; }
}
