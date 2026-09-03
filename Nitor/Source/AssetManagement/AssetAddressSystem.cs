using System.Buffers.Binary;
using System.IO.Hashing;
using System.Text;

namespace Nitor.AssetManagement;

internal class AssetAddressSystem : ISystem
{
	private static readonly Dictionary<UInt128, AssetLocation> _manifest = [];

#if RELEASE
	public void OnCreate()
	{
		using var fs = File.OpenRead(Asset.PackFile);
		var reader = new BinaryReader(fs);

		int manifestEntryCount = reader.ReadInt32(); // Manifest number of entries.

		for (int i = 0; i < manifestEntryCount; i++)
		{
			UInt128 hash = BinaryPrimitives.ReadUInt128BigEndian(reader.ReadBytes(16)); // Hashed version of asset's relative path
			int pos = reader.ReadInt32(); // Pos
			int size = reader.ReadInt32(); // Size

			_manifest.Add(hash, new(pos, size));
		}
	}
#endif

	public static bool TryGetLocation(string path, out int position, out int size)
	{
		path = path.Replace('/', '\\');
		var hash = XxHash128.HashToUInt128(Encoding.UTF8.GetBytes(path));

		if (_manifest.TryGetValue(hash, out var location))
		{
			position = location.Position;
			size = location.Size;
			return true;
		}

		position = -1;
		size = -1;
		return false;
	}
}
