namespace Nitor.Rendering;

public class DynamicBatch : IRenderable
{
	public Shader Shader => throw new NotImplementedException();

	public Texture Texture => throw new NotImplementedException();

	public BlendMode BlendMode => throw new NotImplementedException();

	public RenderLayer Layer => throw new NotImplementedException();

	// Has initial size.
	// Shrink() shrinkwraps it.
	// Grow() increases space.
	// Options to auto grow and auto shrink. Or you can leave it manual.

	public void OnRender()
	{
		throw new NotImplementedException();
	}
}
