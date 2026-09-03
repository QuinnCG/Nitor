namespace Nitor;

public record class AssetLocation(int Position, int Size)
{
	public readonly int Position = Position;
	public readonly int Size = Size;
}
