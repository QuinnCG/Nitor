using StbiSharp;

namespace AssetPacker;

class ImageConverter : AssetConverter
{
	public override string[] Extensions { get; } = ["png", "jpg", "jpeg", "bmp"];

	public override async Task<byte[]> ConvertAsync(FileStream stream)
	{
		Stbi.SetFlipVerticallyOnLoad(true);

		var data = await ReadAllBytesAsync(stream);
		var imgData = Stbi.LoadFromMemory(new ReadOnlySpan<byte>(data), 4);

		var writer = new BinaryWriter(new MemoryStream());

		writer.Write(imgData.Width); // Width (4 bytes)
		writer.Write(imgData.Height); // Height (4 bytes)
		writer.Write(imgData.NumChannels); // Channels (4 bytes)
		writer.Write(imgData.Data.Length); // Pixel data length (4 bytes)
		writer.Write(imgData.Data); // Pixels (variable)

		return await ReadAllBytesAsync(writer.BaseStream);
	}
}
