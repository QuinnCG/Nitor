namespace Nitor.Rendering;

public interface IRenderable
{
	public Shader Shader { get; }
	public Texture Texture { get; }
	public BlendMode BlendMode { get; }
	public RenderLayer Layer { get; }

	public void OnRender();
}
