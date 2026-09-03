using Nitor;

namespace AssetPacker;

/// <summary>
/// Raw copies data over. Used for things like text files, but also as a fallback.
/// </summary>
class DefaultConverter : AssetConverter
{
	public override string[] Extensions { get; } = [string.Empty];

	public override async Task<byte[]> ConvertAsync(FileStream stream)
	{
		return await ReadAllBytesAsync(stream);
	}
}
