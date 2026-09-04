namespace Nitor.Rendering;

public class DynamicBatch : IRenderable
{
	public Shader Shader { get; set; } = null!;
	public Texture Texture { get; set; } = Texture.NoTexture;
	public BlendMode BlendMode { get; set; } = BlendMode.Opaque;
	public RenderLayer Layer { get; set; } = RenderLayer.Default;

	public void OnRender()
	{
		throw new NotImplementedException();
	}

	// Has initial size.
	// Shrink() shrinkwraps it.
	// Grow() increases space.
	// Options to auto grow and auto shrink. Or you can leave it manual.
}
