using System.Text;
using Nitor.AssetManagement;

namespace Nitor;

public class TextFile : Asset
{
	public static Encoding Encoding { get; set; } = Encoding.UTF8;

	public string Text { get; set; } = null!;

	protected override void OnLoad(byte[] data)
	{
		Text = Encoding.GetString(data);
	}

	protected override Task<byte[]> OnSaveAsync()
	{
		return Task.FromResult(Encoding.GetBytes(Text));
	}

	protected override void OnUnload()
	{
		Text = null!;
	}
}
