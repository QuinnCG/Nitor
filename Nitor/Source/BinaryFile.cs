using Nitor.AssetManagement;

namespace Nitor;

public class BinaryFile : Asset
{
	public byte[] Data { get; set; } = null!;

	protected override void OnLoad(byte[] data)
	{
		Data = data;
	}

	protected override Task<byte[]> OnSaveAsync()
	{
		return Task.FromResult(Data);
	}

	protected override void OnUnload()
	{
		Data = null!;
	}
}
