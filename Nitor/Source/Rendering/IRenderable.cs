namespace Nitor.Rendering;

public interface IRenderable
{
	public Shader Shader { get; }
	public BlendMode BlendMode { get; }
	public RenderLayer Layer { get; }

	public void OnRender();
}
