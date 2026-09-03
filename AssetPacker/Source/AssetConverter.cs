namespace AssetPacker;

abstract class AssetConverter
{
	public abstract string[] Extensions { get; }

	/// <summary>
	/// Read from the file stream and return the expected data in bytes.<br/>
	/// There is no need to dispose of the file stream as that is handled automatically.<br/>
	/// All given file streams will be for assets with the extension specified in <see cref="Extensions"/>.
	/// </summary>
	public abstract Task<byte[]> ConvertAsync(FileStream stream);

	protected static async Task<byte[]> ReadAllBytesAsync(Stream stream)
	{
		var data = new byte[stream.Length];
		await stream.ReadExactlyAsync(data, 0, data.Length);
		return data;
	}
}
