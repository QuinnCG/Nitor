using System.IO.Hashing;
using System.Linq.Expressions;
using System.Text;
using Nitor.AssetManagement;

namespace AssetPacker;

public static class Packer
{
	// Maps: File Extension to Converter.
	private static readonly Dictionary<string, AssetConverter> _converters = [];

	public static async Task PackAsync(string sourcePath, string targetPath)
	{
		RegisterConverters();
		var assets = await ConvertAssetsAsync(sourcePath);

		const int manifestEntrySize = 24;
		int manifestSize = manifestEntrySize * assets.Count + sizeof(int); // The int is for the initial bytes that equate to entry count.

		var assetData = CreateAssetData(assets, manifestSize, out var mapping);
		var manifest = CreateManifest(assets, mapping);

		var memory = new MemoryStream();
		manifest.Position = 0;
		manifest.CopyTo(memory);
		assetData.Position = 0;
		assetData.CopyTo(memory);

		var packData = new byte[memory.Length];
		memory.Position = 0;
		memory.ReadExactly(packData, 0, packData.Length);
		targetPath = targetPath.Remove(targetPath.Length - 1); // Remove the trailing period. Used to avoid escaping the quotes in the commandline.
		File.WriteAllBytes(targetPath + Asset.PackFile, packData);

		var memoryHandle = memory.DisposeAsync();
		var manifestHandle = manifest.DisposeAsync();
		var assetDataHandle = assetData.DisposeAsync();

		await memoryHandle;
		await manifestHandle;
		await assetDataHandle;
	}

	private static void RegisterConverters()
	{
		Register<DefaultConverter>();
		Register<ImageConverter>();
	}

	private static Stream CreateAssetData(List<AssetData> assets, int manifestSize, out Dictionary<string, AssetLocation> assetToLocation)
	{
		assetToLocation = new();

		int positionOffset = manifestSize;
		var writer = new BinaryWriter(new MemoryStream());

		foreach (var asset in assets)
		{
			assetToLocation.Add(asset.RelativePath, new(positionOffset + (int)writer.BaseStream.Position, asset.Data.Length));
			writer.Write(asset.Data);
		}

		return writer.BaseStream;
	}

	private static Stream CreateManifest(List<AssetData> assets, Dictionary<string, AssetLocation> mapping)
	{
		var writer = new BinaryWriter(new MemoryStream());
		writer.Write(assets.Count); // Manifest entry count (4 bytes).

		for (var i = 0; i < assets.Count; i++)
		{
			var asset = assets[i];
			var location = mapping[asset.RelativePath];
			var hash = XxHash128.Hash(Encoding.UTF8.GetBytes(asset.RelativePath));

			writer.Write(hash); // Relative Path (16 bytes)
			writer.Write(location.Position); // Start pos (4 bytes)
			writer.Write(location.Size); // Size of data (4 bytes)
		}

		return writer.BaseStream;
	}

	private static async Task<List<AssetData>> ConvertAssetsAsync(string sourcePath)
	{
		var tasks = new List<(string Path, Task<byte[]> Task)>();

		var uniqueConverters = new HashSet<AssetConverter>();

		foreach (var converter in _converters.Values)
		{
			uniqueConverters.Add(converter);
		}

		foreach (var converter in uniqueConverters)
		{
			converter.OnInitialize();
		}

		foreach (var path in Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories))
		{
			string relativePath = path
				.Replace(sourcePath, string.Empty) // Make path relative.
				.Remove(0, 1); // Removing leading slash.

			var parts = relativePath.Split('.');
			string extension = (parts.Length > 1) ? parts[^1] : string.Empty;

			var task = PackAssetAsync(path, extension);
			tasks.Add((relativePath, task));
		}

		var allData = new List<AssetData>();
		var builder = new StringBuilder();

		foreach (var (path, task) in tasks)
		{
			await task;
			var data = task.Result;

			allData.Add(new()
			{
				RelativePath = path,
				Data = data
			});

			builder.AppendLine($"\t- '{path}' ({data.Length:N0} bytes)");
		}

		foreach (var converter in uniqueConverters)
		{
			converter.OnCleanUp();
		}

		Console.Write(builder.ToString());

		return allData;
	}

	private static void Register<T>() where T : AssetConverter, new()
	{
		var converter = new T();

		foreach (var extension in converter.Extensions)
		{
			_converters.Add(extension, converter);
		}
	}

	private static async Task<byte[]> PackAssetAsync(string assetPath, string extension)
	{
		using var fs = File.OpenRead(assetPath);
		var converter = GetConverter(extension);
		byte[] data = await converter.ConvertAsync(fs);
		fs.Dispose();

		return data;
	}

	private static AssetConverter GetConverter(string extension)
	{
		if (!_converters.TryGetValue(extension, out var converter))
		{
			// Use default converter as fallback.
			converter = _converters[string.Empty];
		}

		return converter;
	}
}
