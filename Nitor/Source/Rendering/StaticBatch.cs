namespace Nitor.Rendering;

public class StaticBatch : IRenderable
{
	// Fixed size. Can manipulate data within fixed size. Optional flags to note if you do not intend to change data (optimization on OpenGLs side).

	public Shader Shader => throw new NotImplementedException();

	public Texture Texture => throw new NotImplementedException();

	public BlendMode BlendMode => throw new NotImplementedException();

	public RenderLayer Layer => throw new NotImplementedException();

	public void OnRender()
	{
		throw new NotImplementedException();
	}
}
