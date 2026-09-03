using System.Text;

namespace Nitor;

public class TextFile : Asset
{
	public static Encoding Encoding { get; set; } = Encoding.UTF8;

	public string Text { get; private set; } = null!;

	protected override void OnLoad(byte[] data)
	{
		Text = Encoding.GetString(data);
	}

	protected override void OnUnload()
	{
		Text = null!;
	}
}
