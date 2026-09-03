namespace Nitor.AssetManagement;

public abstract class Asset
{
	public static readonly string PackFile = "Data.pak";

	class AssetData
	{
		public required Asset Asset;
		public int ReferenceCount;
	}

	public string Path { get; private set; } = null!;

	private static readonly Dictionary<string, AssetData> _loadedAssets = [];

	public static T Load<T>(string path) where T : Asset, new()
	{
		static T Converted(Asset asset)
		{
			if (asset is T converted)
			{
				return converted;
			}
			else
			{
				throw new Exception("Cannot load asset with the specified type!");
			}
		}

		if (_loadedAssets.TryGetValue(path, out var assetData))
		{
			assetData.ReferenceCount++;
			return Converted(assetData.Asset);
		}

		var newAsset = Create<T>(path, ResolveAssetData(path));
		return newAsset;
	}

	public static T Create<T>(string path, byte[] data) where T : Asset, new()
	{
		if (_loadedAssets.ContainsKey(path))
		{
			throw new Exception("Asset at path already exists!");
		}

		var asset = new T() { Path = path };
		asset.OnLoad(data);

		_loadedAssets.Add(path, new()
		{
			Asset = asset,
			ReferenceCount = 1
		});

		return asset;
	}

	public static void Release(string path)
	{
		if (_loadedAssets.TryGetValue(path, out var assetData))
		{
			assetData.ReferenceCount--;

			if (assetData.ReferenceCount <= 0)
			{
				assetData.Asset.OnUnload();
				_loadedAssets.Remove(path);
			}
		}
	}

	private static byte[] ResolveAssetData(string path)
	{
#if DEBUG
		return File.ReadAllBytes($"Assets\\{path}");
#else
		if (AssetAddressSystem.TryGetLocation(path, out int pos, out int size))
		{
			using var fs = File.OpenRead(PackFile);
			fs.Position = pos;

			var data = new byte[size];
			fs.ReadExactly(data, 0, size);

			return data;
		}
		else
		{
			throw new Exception("Failed to resolve asset at path!");
		}
#endif
	}

	public void Release()
	{
		Release(Path);
	}

#pragma warning disable CA1822 // Mark members as static
	public async Task SaveAsync()
#pragma warning restore CA1822 // Mark members as static
	{
#if DEBUG
		var saveHandle = OnSaveAsync();

		if (saveHandle == null)
		{
			throw new Exception("Cannot save asset as it does not implement its OnSaveAsync() method!");
		}

		var data = await saveHandle;
		var localWrite = File.WriteAllBytesAsync($"Assets\\{Path}", data);
		var originWrite = File.WriteAllBytesAsync($"../../../Assets\\{Path}", data);

		await localWrite;
		await originWrite;
#else
		throw new Exception("Cannot save data in Release builds!");
#endif
	}
	public async void Save()
	{
		SaveAsync().Wait();
	}

	protected virtual void OnLoad(byte[] data) { }
	protected virtual void OnUnload() { }

	protected virtual Task<byte[]> OnSaveAsync() => null!;

	public override bool Equals(object? obj)
	{
		return obj is Asset asset && asset.Path == Path;
	}

	public override int GetHashCode()
	{
		return Path.GetHashCode();
	}

	public override string ToString()
	{
		return Path;
	}
}
