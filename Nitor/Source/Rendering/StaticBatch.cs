namespace Nitor.Rendering;

public class StaticBatch : IRenderable
{
	// Fixed size. Can manipulate data within fixed size. Optional flags to note if you do not intend to change data (optimization on OpenGLs side).

	public Shader Shader { get; set; } = null!;
	public Texture Texture { get; set; } = Texture.NoTexture;
	public BlendMode BlendMode { get; set; } = BlendMode.Opaque;
	public RenderLayer Layer { get; set; } = RenderLayer.Default;

	public void OnRender()
	{
		
	}
}
